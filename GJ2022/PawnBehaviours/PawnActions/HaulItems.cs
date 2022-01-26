using GJ2022.Areas;
using GJ2022.Entities.Items;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.PawnBehaviours.PawnActions
{
    class HaulItems : PawnAction
    {

        private enum HaulItemActionModes
        {
            FETCH_ITEM,
            DELIVER_ITEM
        }

        public override bool Overriding => false;

        public override int Priority => 20;

        //Unreachable item locations
        //We maintain this memory until we pause the action due to not having items available.
        //Once the action is cancelled, we forget about anything we couldn't reach (world could have changed by then)
        private HashSet<Vector<int>> unreachablePositions = new HashSet<Vector<int>>();

        //Current item target
        private Item haulTarget;

        //The current state
        private HaulItemActionModes haulItemActionState;

        //Have we completed?
        private bool completed = false;

        public override bool CanPerform(PawnBehaviour parent)
        {
            if (World.WorldAreas.Count == 0)
                return false;
            //Check the world for items not in a stockpile
            foreach (Vector<int> itemPosition in World.WorldItems.Keys)
            {
                //A haulable item is not in a stockpile & is not unreachable
                //TODO: Check items to see if we actually want to haul them or not
                if (!(World.GetArea(itemPosition[0], itemPosition[1]) is StockpileArea) && !unreachablePositions.Contains(itemPosition))
                {
                    return true;
                }
            }
            //If there are no items available, pause this action for 30 seconds
            parent.PauseActionFor(this, 30);
            unreachablePositions.Clear();
            Log.WriteLine("Failed to locate haulable items in the world, cancelling haul task.");
            return false;
        }

        public override bool Completed(PawnBehaviour parent)
        {
            return completed;
        }

        public override void OnActionCancel(PawnBehaviour parent)
        {
            //Drop the pawn's held item
            parent.Owner.DropHeldItems();
        }

        public override void OnActionEnd(PawnBehaviour parent)
        {
            //Clear memory
            if(haulTarget != null)
                haulTarget.claim = null;
            haulTarget = null;
            completed = false;
        }

        public override void OnActionStart(PawnBehaviour parent)
        {
            //If we are holding an item, just deliver it
            if (!parent.Owner.HasFreeHands())
            {
                PathToFreeStockpile(parent);
            }
            //Else, locate an item to haul
            else
            {
                PathToItem(parent);
            }
        }

        public override void OnActionUnreachable(PawnBehaviour parent)
        {
            //Drop held items
            parent.Owner.DropHeldItems();
            unreachablePositions.Add(haulTarget.Position);
            completed = true;
        }

        public override void OnPawnReachedLocation(PawnBehaviour parent)
        {
            Log.WriteLine($"Reached location, doing something ({haulItemActionState})...");
            if (haulTarget != null && (parent.Owner.Position != haulTarget.Position || haulTarget.Location != null))
            {
                //Target was moved
                if (haulTarget.claim == parent.Owner)
                    haulTarget.claim = null;
                haulTarget = null;
                if (parent.Owner.HasFreeHands())
                    PathToItem(parent);
                else
                    //Path to a free stockpile
                    PathToFreeStockpile(parent);
                return;
            }
            switch (haulItemActionState)
            {
                case HaulItemActionModes.FETCH_ITEM:
                    //Check if the item is still here
                    if (parent.Owner.Position != haulTarget.Position)
                    {
                        //Someone beat us to it
                        completed = true;
                        return;
                    }
                    //Pickup the item
                    if (!parent.Owner.TryPickupItem(haulTarget))
                    {
                        //Try to free some space
                        parent.Owner.DropHeldItems();
                        //Try to pick it up again, now with free hands
                        if (!parent.Owner.TryPickupItem(haulTarget))
                        {
                            //Something went wrong
                            completed = true;
                            return;
                        }
                    }
                    //Destination reached, nullify haul target
                    haulTarget.claim = null;
                    haulTarget = null;
                    //If we can carry more, find more items to haul
                    if (parent.Owner.HasFreeHands())
                        PathToItem(parent);
                    else
                        //Path to a free stockpile
                        PathToFreeStockpile(parent);
                    return;
                case HaulItemActionModes.DELIVER_ITEM:
                    Log.WriteLine("Items delivered!");
                    //Drop the item at the current location
                    parent.Owner.DropHeldItems();
                    //Mark as completed
                    completed = true;
                    return;
            }
        }

        /// <summary>
        /// Path to the nearest item on the ground.
        /// If we cannot locate any items on the ground, path towards a stockpile zone if we are holding something.
        /// </summary>
        private void PathToItem(PawnBehaviour parent)
        {
            Item locatedItem = null;
            //Locate the nearest free item within a radius
            foreach (Item item in World.GetItemsInRange((int)parent.Owner.Position[0], (int)parent.Owner.Position[1], 50))
            {
                //Skip over items inside something
                if (item.Location != null)
                    continue;
                //Skip over items inside a stockpile
                if (World.GetArea((int)item.Position[0], (int)item.Position[1]) is StockpileArea)
                    continue;
                //Skip claimed items
                if (item.claim != null)
                    continue;
                locatedItem = item;
                break;
            }
            //If nothing was found, fail the action
            if (locatedItem == null)
            {
                //If we are holding something, go to the deliver phase
                if (parent.Owner.IsHoldingItems())
                {
                    PathToFreeStockpile(parent);
                    return;
                }
                //Otherwise the action can be paused for 30 seconds
                completed = true;
                parent.PauseActionFor(this, 30);
                unreachablePositions.Clear();
                Log.WriteLine("Failed to locate items to haul, cancelling haul task.");
                return;
            }
            Log.WriteLine($"Pathing to item at {locatedItem.Position} inside {locatedItem.Location}...");
            //Alright, off we go
            if(haulTarget != null)
                haulTarget.claim = null;
            haulTarget = locatedItem;
            haulTarget.claim = parent.Owner;
            haulItemActionState = HaulItemActionModes.FETCH_ITEM;
            parent.Owner.MoveTowardsEntity(locatedItem);
        }

        private void PathToFreeStockpile(PawnBehaviour parent)
        {
            //Find a stockpile
            foreach (Vector<int> position in World.WorldAreas.Keys)
            {
                Area area = World.WorldAreas[position];
                //Stockpile is not used
                if (!(area is StockpileArea))
                    continue;
                //Stockpile is used
                if (World.GetItems((int)position[0], (int)position[1]).Count > 0)
                    continue;
                //Path to this location
                haulItemActionState = HaulItemActionModes.DELIVER_ITEM;
                Log.WriteLine("Pathing to stockpile...");
                parent.Owner.MoveTowardsPosition(position);
                return;
            }
            //Finding stockpile failed.
            //This action failed!
            completed = true;
            parent.PauseActionFor(this, 30);
            unreachablePositions.Clear();
            Log.WriteLine("Failed to locate stockpile in range, cancelling haul task.");
        }

        //Check if the item we are pathing towards still exists
        //Check if the stockpile zone we chose to go towards is still empty
        public override void PerformProcess(PawnBehaviour parent)
        {
            return;
        }
    }
}
