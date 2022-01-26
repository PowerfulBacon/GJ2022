using GJ2022.Entities.Blueprints;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items;
using GJ2022.Game.GameWorld;
using GJ2022.Managers.Stockpile;
using GJ2022.Pathfinding;
using GJ2022.PawnBehaviours;
using GJ2022.Rendering.RenderSystems.LineRenderer;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Subsystems;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GJ2022.Entities.Pawns
{
    public class Pawn : Entity, IProcessable
    {

        //The renderable attached to our pawn
        protected override Renderable Renderable { get; set; } = new CircleRenderable(Colour.Random);

        //TODO: Draw debug pathfinding lines
        public static bool DrawLines = false;

        //Is this pawn destroyed?
        public bool Destroyed { get; set; } = false;

        //The AI controller
        public PawnBehaviour behaviourController;

        //Held items
        public Item[] heldItems = new Item[2];

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

        private void DrawHelpfulLine()
        {
            Vector<float> endPos = targetDestinationPosition;
            if (!hasTargetDestination)
            {
                helpfulLine?.StopDrawing();
                helpfulLine = null;
                return;
            }
            if (helpfulLine == null)
                helpfulLine = Line.StartDrawingLine(Position.SetZ(10), endPos.SetZ(10));
            helpfulLine.Start = Position.SetZ(10);
            helpfulLine.End = endPos.SetZ(10);
        }

        public bool TryPickupItem(Item item)
        {
            //Can't pickup if the item was moved somewhere else
            if (item.Location != null)
                return false;
            //Can't pick up from too far away
            if ((item.Position - Position).Length() > 0.5f)
                return false;
            //Hands check
            int freeIndex = -1;
            for (int i = heldItems.Length - 1; i >= 0; i--)
            {
                if (heldItems[i] == null)
                {
                    freeIndex = i;
                    break;
                }
            }
            //If we have no hands return false
            if (freeIndex == -1)
                return false;
            //Pickup the item
            heldItems[freeIndex] = item;
            item.Location = this;
            Log.WriteLine($"Picked up {item}, it is now stored within {item.Location}");
            return true;
        }

        public bool HasFreeHands()
        {
            for (int i = heldItems.Length - 1; i >= 0; i--)
            {
                Log.WriteLine($"{i} - {heldItems[i]}");
                if (heldItems[i] == null)
                    return true;
            }
            return false;
        }

        public bool IsHoldingItems()
        {
            for (int i = heldItems.Length - 1; i >= 0; i--)
            {
                if (heldItems[i] != null)
                    return true;
            }
            return false;
        }

        public void DropHeldItems()
        {
            for (int i = heldItems.Length - 1; i >= 0; i--)
            {
                if (heldItems[i] == null)
                    continue;
                //Drop the item out of ourselves
                heldItems[i].Location = null;
                heldItems[i].Position = Position;
                heldItems[i].claim = null;
                heldItems[i] = null;
            }
        }

        public void Process(float deltaTime)
        {
            DrawHelpfulLine();
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
        private void TraversePath(float deltaTime, float distance = 0.1f)
        {
            //Check if our target moved
            if (entityTargetDestination != null && (entityTargetDestination.Position != targetDestinationPosition || entityTargetDestination.Location != null))
            {
                //Our target was moved :(
                entityTargetDestination = null;
                hasTargetDestination = false;
                followingPath = null;
                positionOnPath = 0;
                behaviourController.PawnActionUnreachable();
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
            float extraDistance;
            Position = Position.MoveTowards(nextPathPosition, distance, deltaTime, out extraDistance);
            //If we reached the point, move towards the next point
            if (Position == nextPathPosition)
            {
                //Increment the path position
                positionOnPath++;
                //If we still have more distance to move, move it
                if(extraDistance > 0)
                    TraversePath(deltaTime, extraDistance * deltaTime);
            }
        }

        /// <summary>
        /// Trigger the AI reach destination intercept
        /// </summary>
        private void ReachDestination()
        {
            Log.WriteLine("Reached destination");
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
                    (Path path) =>
                    {
                        followingPath = path;
                        positionOnPath = 0;
                        waitingForPath = false;
                    },
                    () => {
                        behaviourController.PawnActionUnreachable();
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
            Destroyed = true;
            return true;
        }

    }
}
