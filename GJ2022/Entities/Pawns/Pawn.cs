using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.ComponentInterfaces.MouseEvents;
using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Game.GameWorld;
using GJ2022.Managers;
using GJ2022.Pathfinding;
using GJ2022.PawnBehaviours;
using GJ2022.Rendering.RenderSystems.LineRenderer;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Subsystems;
using GJ2022.Subsystems.Processing;
using GJ2022.Utility.MathConstructs;
using System;

namespace GJ2022.Entities.Pawns
{
    public abstract partial class Pawn : Entity, IProcessable, IMousePress, IMoveBehaviour
    {

        //The renderable attached to our pawn
        public override Renderable Renderable { get; set; } = new CircleRenderable(Colour.Random);

        //TODO: Draw debug pathfinding lines
        public static bool DrawLines = false;

        //Is this pawn destroyed?
        public bool Destroyed { get; set; } = false;

        public CursorSpace PositionSpace => CursorSpace.WORLD_SPACE;

        public float WorldX => Position[0] - 0.5f;

        public float WorldY => Position[1] - 0.5f;

        public float Width => 1.0f;

        public float Height => 1.0f;

        public virtual bool UsesLimbOverlays { get; } = false;

        //The body attached to this pawn
        public abstract Body PawnBody { get; }

        //The AI controller
        public PawnBehaviour behaviourController;

        //The intended destination
        private Entity entityTargetDestination;
        //Positional destination
        //Entity target destination takes precidence over this
        private Vector<float> targetDestinationPosition;
        //Do we have a target destination
        private bool hasTargetDestination = false;
        //Are we waiting for a path?
        private bool waitingForPath = false;
        //The path we are currently following
        private Path followingPath = null;
        //The position we currently are along this path
        private int positionOnPath;

        private Line helpfulLine;

        public Pawn(Vector<float> position) : base(position, Layers.LAYER_PAWN)
        {
            PawnControllerSystem.Singleton.StartProcessing(this);
            BodyProcessingSystem.Singleton.StartProcessing(PawnBody);
            MouseCollisionSubsystem.Singleton.StartTracking(this);
            PawnBody.SetupBody(this);
        }

        public void MoveTowardsEntity(Entity target)
        {
            //Set our new target destination
            entityTargetDestination = target;
            //Set the position to go to
            targetDestinationPosition = target.Position.Copy();
            hasTargetDestination = true;
            //Nullify the path we were following (calculate a new one)
            followingPath = null;
        }

        public void MoveTowardsPosition(Vector<float> position)
        {
            //Set our new target destination
            targetDestinationPosition = position.Copy();
            hasTargetDestination = true;
            //Nullify the entity target destination
            entityTargetDestination = null;
            //Nullify the path we were following (calculate a new one)
            followingPath = null;
        }

        public bool HasTarget()
        {
            return hasTargetDestination;
        }

        private void DrawHelpfulLine()
        {
            Vector<float> endPos = targetDestinationPosition;
            if (!hasTargetDestination || !DrawLines)
            {
                helpfulLine?.StopDrawing();
                helpfulLine = null;
                return;
            }
            if (helpfulLine == null)
                helpfulLine = Line.StartDrawingLine(Position.SetZ(10), endPos.SetZ(10));
            helpfulLine.Start = Position.SetZ(10);
            helpfulLine.End = endPos.SetZ(10);
            helpfulLine.Colour = followingPath != null ? Colour.Green : Colour.Red;
        }

        public void Process(float deltaTime)
        {
            //Draw debug lines
            DrawHelpfulLine();
            //If dead
            if (Dead)
                return;
            //If we are in crit don't bother with this stuff
            if (InCrit)
            {
                //Update animation
                Renderable.UpdateRotation((float)(Math.Sin(TimeManager.Time) * 0.3f + Math.PI * 0.5f));
                //End procesing here
                return;
            }
            //Hazard reaction, prepare to panic
            HazardReact();
            //Idle behaviour
            if (IsIdle())
                return;
            //Wait for pathfinding
            if (IsWaitingForPath())
                return;
            //Traverse along the path
            TraversePath(deltaTime);
        }

        public bool IsIdle()
        {
            if (hasTargetDestination)
                return false;
            //Do idle behaviours
            return true;
        }

        /// <summary>
        /// Travel along the specified path
        /// </summary>
        private void TraversePath(float deltaTime, float _speed = -1)
        {
            float speed = _speed;
            if (speed == -1)
            {
                speed = CalculateSpeed();
            }
            //Check if our target moved
            if (entityTargetDestination != null && (entityTargetDestination.Position != targetDestinationPosition || entityTargetDestination.Location != null))
            {
                //Our target was moved :(
                entityTargetDestination = null;
                hasTargetDestination = false;
                followingPath = null;
                positionOnPath = 0;
                return;
            }
            //We have reached our destination
            if (Position == targetDestinationPosition)
            {
                ReachDestination();
                return;
            }
            //Move towards the next node on our path
            Vector<float> nextPathPosition = positionOnPath < followingPath.Points.Count
                ? (Vector<float>)followingPath.Points[positionOnPath]
                : targetDestinationPosition;
            //Move towards the point
            Position = Position.MoveTowards(nextPathPosition, speed, deltaTime, out float extraDistance);
            //If we reached the point, move towards the next point
            if (Position == nextPathPosition)
            {
                //Increment the path position
                positionOnPath++;
            }
            //If we still have more distance to move, move it
            if (extraDistance > 0)
            {
                //We need to actually pass the distance travelled, while extraDistance is a speed
                TraversePath(deltaTime, extraDistance * deltaTime);
            }
        }

        private float CalculateSpeed()
        {
            //If we have gravity return movement factor
            if (World.HasGravity(Position))
                return 0.04f * PawnBody.Movement;
            //Return regular speed
            return 4f;
        }

        /// <summary>
        /// Trigger the AI reach destination intercept
        /// </summary>
        private void ReachDestination()
        {
            hasTargetDestination = false;
            entityTargetDestination = null;
            followingPath = null;
            positionOnPath = 0;
            behaviourController.PawnActionReached();
        }

        /// <summary>
        /// Returns true if we are waiting for a path.
        /// Returns false if we have a path.
        /// Automatically queues pathfinding if we don't have a path and need one
        /// </summary>
        /// <returns></returns>
        private bool IsWaitingForPath()
        {
            //We are already waiting for a path
            if (waitingForPath)
                return true;
            //We have a path
            if (followingPath != null)
                return false;
            //We need to start calculating a path
            waitingForPath = true;
            PathfindingSystem.Singleton.RequestPath(
                new PathfindingRequest(
                    Position,
                    targetDestinationPosition,
                    //If we have an internal tank, allow pathing through airless areas
                    cachedHazardProtection | (HasInternalTank() ? PawnHazards.HAZARD_BREATH : PawnHazards.NONE) | (IsPressureProtected() ? PawnHazards.HAZARD_LOW_PRESSURE : PawnHazards.NONE),
                    (Path path) =>
                    {
                        followingPath = path;
                        positionOnPath = 0;
                        waitingForPath = false;
                    },
                    () =>
                    {
                        behaviourController.PawnActionUnreachable(targetDestinationPosition);
                        waitingForPath = false;
                    }
                ));
            return true;
        }

        private void HazardReact()
        {
            //Detect unhandled hazards
            //behaviourController.PawnActionIntercept();
        }

        public override bool Destroy()
        {
            base.Destroy();
            PawnControllerSystem.Singleton.StopProcessing(this);
            BodyProcessingSystem.Singleton.StopProcessing(PawnBody);
            Destroyed = true;
            return true;
        }

        public void OnPressed()
        {
            PawnControllerSystem.Singleton.SelectPawn(this);
        }

        /// <summary>
        /// On move behaviour.
        /// Update ourself in the world list
        /// </summary>
        public void OnMoved(Vector<float> oldPosition)
        {
            if ((int)oldPosition[0] == (int)Position[0] && (int)oldPosition[1] == (int)Position[1])
                return;
            World.RemovePawn((int)oldPosition[0], (int)oldPosition[1], this);
            World.AddPawn((int)Position[0], (int)Position[1], this);
        }

        public void OnMoved(Entity oldLocation)
        {
            if (oldLocation == Location)
                return;
            World.RemovePawn((int)Position[0], (int)Position[1], this);
        }

    }
}
