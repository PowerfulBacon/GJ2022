﻿using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.ComponentInterfaces.MouseEvents;
using GJ2022.Entities.Items;
using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Game.GameWorld;
using GJ2022.Managers.TaskManager;
using GJ2022.Pathfinding;
using GJ2022.PawnBehaviours;
using GJ2022.Rendering.RenderSystems.LineRenderer;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.Entities.Pawns
{
    public class Pawn : Entity, IProcessable, IMousePress, IMoveBehaviour
    {

        //The renderable attached to our pawn
        protected override Renderable Renderable { get; set; } = new CircleRenderable(Colour.Random);

        //TODO: Draw debug pathfinding lines
        public static bool DrawLines = false;

        //Is this pawn destroyed?
        public bool Destroyed { get; set; } = false;

        public CursorSpace PositionSpace => CursorSpace.WORLD_SPACE;

        public float WorldX => Position[0] - 0.5f;

        public float WorldY => Position[1] - 0.5f;

        public float Width => 1.0f;

        public float Height => 1.0f;

        //The body attached to this pawn
        public Body PawnBody { get; }

        //The AI controller
        public PawnBehaviour behaviourController;

        //Equipped items
        public Dictionary<InventorySlot, IEquippable> EquippedItems = new Dictionary<InventorySlot, IEquippable>();
        //Flags of hazards we are protected from due to our equipped items
        private PawnHazards cachedHazardProtection = PawnHazards.NONE;

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
            MouseCollisionSubsystem.Singleton.StartTracking(this);
        }

        /// <summary>
        /// Thread safe item equip
        /// </summary>
        public bool TryEquipItem(InventorySlot targetSlot, IEquippable item)
        {
            return ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_PAWN_EQUIPPABLES, () =>
            {
                //Check the slot
                if (EquippedItems.ContainsKey(targetSlot))
                    return false;
                EquippedItems.Add(targetSlot, item);
                item.OnEquip(this, targetSlot);
                RecalculateHazardProtection();
                AddEquipOverlay(targetSlot, item);
                return true;
            });
        }

        protected virtual void AddEquipOverlay(InventorySlot targetSlot, IEquippable item) { }

        /// <summary>
        /// Recalculate what hazards we are protected from
        /// </summary>
        private void RecalculateHazardProtection()
        {
            cachedHazardProtection = PawnHazards.NONE;
            foreach (IEquippable item in EquippedItems.Values)
            {
                cachedHazardProtection |= item.ProtectedHazards;
            }
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

        public List<Item> GetHeldItems()
        {
            List<Item> items = new List<Item>();
            for (int i = 0; i < heldItems.Length; i++)
            {
                if (heldItems[i] == null)
                    continue;
                if (heldItems[i].Destroyed || heldItems[i].Location != this)
                {
                    heldItems[i] = null;
                    continue;
                }
                items.Add(heldItems[i]);
            }
            return items;
        }

        public bool TryPickupItem(Item item)
        {
            return ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_PAWN_INVENTORY, () =>
            {
                //Destroyed items cannot be picked up
                if (item.Destroyed)
                    return false;
                //Can't pickup if the item was moved somewhere else
                if (!InReach(item))
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
                return true;
            });
        }

        public bool HasFreeHands()
        {
            return ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_PAWN_INVENTORY, () =>
            {
                for (int i = heldItems.Length - 1; i >= 0; i--)
                {
                    if (heldItems[i] == null)
                        return true;
                    if (heldItems[i].Destroyed)
                    {
                        heldItems[i] = null;
                        return true;
                    }
                }
                return false;
            });
        }

        public bool IsHoldingItems()
        {
            return ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_PAWN_INVENTORY, () =>
            {
                for (int i = heldItems.Length - 1; i >= 0; i--)
                {
                    if (heldItems[i] != null)
                    {
                        if (!heldItems[i].Destroyed)
                            return true;
                        heldItems[i] = null;
                    }
                }
                return false;
            });
        }

        public bool DropFirstItem(Vector<float> dropLocation)
        {
            return ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_PAWN_INVENTORY, () =>
            {
                for (int i = heldItems.Length - 1; i >= 0; i--)
                {
                    if (heldItems[i] == null)
                        continue;
                    if (heldItems[i].Destroyed)
                    {
                        heldItems[i] = null;
                        continue;
                    }
                    //Drop the item out of ourselves
                    heldItems[i].Location = null;
                    heldItems[i].Position = dropLocation.Copy();
                    heldItems[i] = null;
                    return true;
                }
                return false;
            });
        }

        public void DropHeldItems(Vector<float> dropLocation)
        {
            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_PAWN_INVENTORY, () =>
            {
                for (int i = heldItems.Length - 1; i >= 0; i--)
                {
                    if (heldItems[i] == null)
                        continue;
                    if (heldItems[i].Destroyed)
                    {
                        heldItems[i] = null;
                        continue;
                    }
                    //Drop the item out of ourselves
                    heldItems[i].Position = dropLocation.Copy();
                    heldItems[i].Location = null;
                    heldItems[i] = null;
                }
                return true;
            });
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
            Position = Position.MoveTowards(nextPathPosition, distance, deltaTime, out extraDistance);
            //If we reached the point, move towards the next point
            if (Position == nextPathPosition)
            {
                //Increment the path position
                positionOnPath++;
                //If we still have more distance to move, move it
                if (extraDistance > 0)
                    TraversePath(deltaTime, extraDistance * deltaTime);
            }
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
                    cachedHazardProtection,
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
