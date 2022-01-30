using GJ2022.Areas;
using GJ2022.Entities.Items;
using GJ2022.Game.GameWorld;
using GJ2022.Managers.TaskManager;
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

        public override int Priority { get; } = 20;

        //Unreachable item locations
        //We maintain this memory until we pause the action due to not having items available.
        //Once the action is cancelled, we forget about anything we couldn't reach (world could have changed by then)
        private HashSet<Vector<int>> unreachablePositions = new HashSet<Vector<int>>();

        //The current state
        private HaulItemActionModes haulActionState = HaulItemActionModes.FETCH_ITEM;

        //Have we completed?
        private bool completed = false;

        private Item forcedTarget;

        public HaulItems() : base() { }

        public HaulItems(Item forcedTarget) : base()
        {
            this.forcedTarget = forcedTarget;
            //Increased priority for force hauls
            Priority = 15;
        }

        public override bool CanPerform(PawnBehaviour parent)
        {
            if (World.HasAreaInRange((int)parent.Owner.Position[0], (int)parent.Owner.Position[1], 40, (Area area) =>{
                if (unreachablePositions.Contains(area.Position))
                    return false;
                if (World.GetArea((int)area.Position[0], (int)area.Position[1]) is StockpileArea)
                    return false;
                return true;
            })
                && World.HasItemsInRange((int)parent.Owner.Position[0], (int)parent.Owner.Position[1], 40, (List<Item> toCheck) =>
                {
                    foreach (Item item in toCheck)
                    {
                        if (unreachablePositions.Contains(item.Position))
                            continue;
                        if (World.GetArea((int)item.Position[0], (int)item.Position[1]) is StockpileArea)
                            continue;
                        return true;
                    }
                    return false;
                }))
            {
                return true;
            }
            unreachablePositions.Clear();
            //This check is intensive, so we will pause for some time
            parent.PauseActionFor(this, 30);
            //Return true if there are haulable items in the world and stockpiles that can hold them
            return false;
        }

        public override bool Completed(PawnBehaviour parent) => completed;

        public override void OnActionStart(PawnBehaviour parent)
        {
            LocateOrStoreItems(parent);
        }

        public override void PerformProcess(PawnBehaviour parent)
        {
            return;
        }

        public override void OnActionCancel(PawnBehaviour parent)
        {
            //Drop whatever we are holding, panic mode activated
            parent.Owner.DropHeldItems(parent.Owner.Position);
        }

        public override void OnActionUnreachable(PawnBehaviour parent, Vector<float> unreachableLocation)
        {
            //Get our claim and mark it as unreachable
            unreachablePositions.Add(unreachableLocation);
            //Unclaim the target
            ThreadSafeClaimManager.ReleaseClaimBlocking(parent.Owner);
            //Choose what to do next
            switch (haulActionState)
            {
                case HaulItemActionModes.DELIVER_ITEM:
                    //Drop what we are holding and cancel this task
                    parent.Owner.DropHeldItems(parent.Owner.Position);
                    parent.PauseActionFor(this, 30);
                    completed = true;
                    return;
                case HaulItemActionModes.FETCH_ITEM:
                    //Just store whatever we are holding
                    StoreHeldItems(parent);
                    return;
            }
        }

        public override void OnPawnReachedLocation(PawnBehaviour parent)
        {
            //We reached the location successfully.
            switch (haulActionState)
            {
                case HaulItemActionModes.DELIVER_ITEM:
                    //We reached our stockpile zone
                    GracefullyDeliver(parent);
                    return;
                case HaulItemActionModes.FETCH_ITEM:
                    //Check for our reserved item
                    Item reservedItem = ThreadSafeClaimManager.GetClaimedItem(parent.Owner) as Item;
                    //Try to pick up the reserved item.
                    if(reservedItem != null)
                        parent.Owner.TryPickupItem(reservedItem);
                    //Now that we have picked up the item, unclaim it
                    ThreadSafeClaimManager.ReleaseClaimBlocking(parent.Owner);
                    //Pick up more items, or move to a storage location
                    LocateOrStoreItems(parent);
                    return;
            }
        }

        public override void OnActionEnd(PawnBehaviour parent)
        {
            //Reset variables
            completed = false;
            //Release our item claim
            ThreadSafeClaimManager.ReleaseClaimBlocking(parent.Owner);
        }

        /// <summary>
        /// We reached our stockpile zone target.
        /// Check if its free and place and item, otherwise move to another.
        /// </summary>
        private void GracefullyDeliver(PawnBehaviour parent)
        {
            StockpileArea claimedArea = ThreadSafeClaimManager.GetClaimedItem(parent.Owner) as StockpileArea;
            //We had no reserved area, or the reserved area was deleted
            if (claimedArea == null)
            {
                StoreHeldItems(parent);
            }
            //Check if we are actually at the claimed area location
            else if (!parent.Owner.InReach(claimedArea, 1))
            {
                //Try to redeliver items
                StoreHeldItems(parent);
            }
            //Check if the claimed area location is still empty
            else if (World.GetItems((int)claimedArea.Position[0], (int)claimedArea.Position[1]).Count > 0)
            {
                //Redeliver items
                StoreHeldItems(parent);
            }
            //Deliver the item
            else
            {
                //Drop a single item at this location and deliver the next
                parent.Owner.DropFirstItem(claimedArea.Position);
                StoreHeldItems(parent);
            }
        }

        /// <summary>
        /// Locate items if we can carry items
        /// Store items if we cannot
        /// </summary>
        private void LocateOrStoreItems(PawnBehaviour parent)
        {
            //Can we carry anything more?
            if (!parent.Owner.HasFreeHands())
            {
                StoreHeldItems(parent);
                return;
            }
            //Attempt to locate items
            List<Item> itemsToSearch = World.GetSprialItems((int)parent.Owner.Position[0], (int)parent.Owner.Position[1], 40);
            Item targetItem = null;
            if (forcedTarget == null)
            {
                //Go through all the items
                foreach (Item item in itemsToSearch)
                {
                    //If the item is claimed, skip it
                    if (item.IsClaimed)
                        continue;
                    //If the item is in an invalid location, skip it
                    if (item.Location != null)
                        continue;
                    //Check if the item is unreachable
                    if (unreachablePositions.Contains(item.Position))
                        continue;
                    //Check if the item
                    //is in a stockpile, if it is, skip it
                    if (World.GetArea((int)item.Position[0], (int)item.Position[1]) as StockpileArea != null)
                        continue;
                    //Looks like the item is valid!
                    targetItem = item;
                    break;
                }
            }
            else
            {
                if (!forcedTarget.IsClaimed && forcedTarget.Location == null && !unreachablePositions.Contains(forcedTarget.Position) && World.GetArea((int)forcedTarget.Position[0], (int)forcedTarget.Position[1]) as StockpileArea == null)
                    targetItem = forcedTarget;
            }
            //No item was located
            if (targetItem == null || targetItem.Location != null)
            {
                //Let's go store whatever we are holding
                StoreHeldItems(parent);
            }
            //Attempt to claim the item
            else if (ThreadSafeClaimManager.ReserveClaimBlocking(parent.Owner, targetItem))
            {
                //Horray, we claimed the item we wanted successfully. Let's go pick it up
                parent.Owner.MoveTowardsEntity(targetItem);
                haulActionState = HaulItemActionModes.FETCH_ITEM;
            }
            else
            {
                //Failed to reserve the item we wanted, try to claim another item
                LocateOrStoreItems(parent);
            }
        }

        /// <summary>
        /// Store held items, even if we are only holding 1/2
        /// </summary>
        private void StoreHeldItems(PawnBehaviour parent)
        {
            //Check if we are holding items to deliver
            if (!parent.Owner.IsHoldingItems())
            {
                //We aren't holding anything, complete the action.
                completed = true;
                return;
            }
            //Locate an unclaimed stockpile zone
            StockpileArea freeStockpileArea = null;
            //Extend range for forced haul
            foreach (Area area in World.GetSprialAreas((int)parent.Owner.Position[0], (int)parent.Owner.Position[1], forcedTarget == null ? 60 : 120))
            {
                //If the area is claimed, skip it
                if (area.IsClaimed)
                    continue;
                //If the area is not a stockpile, skip it
                if (!(area is StockpileArea))
                    continue;
                //Unreachable stockpiles
                if (unreachablePositions.Contains(area.Position))
                    continue;
                //If the area has items in it, skip it
                if (World.GetItems((int)area.Position[0], (int)area.Position[1]).Count > 0)
                    continue;
                //A good stockpile area
                freeStockpileArea = (StockpileArea)area;
                break;
            }
            //If we cannot locate a stockpile zone, cancel this task and drop all items
            if (freeStockpileArea == null)
            {
                OnActionCancel(parent);
                completed = true;
            }
            //If we can locate a stockpile zone, attempt to claim it and path towards it
            else if (ThreadSafeClaimManager.ReserveClaimBlocking(parent.Owner, freeStockpileArea))
            {
                //Our claim was successful, let's go there and have fun
                parent.Owner.MoveTowardsPosition(freeStockpileArea.Position);
                haulActionState = HaulItemActionModes.DELIVER_ITEM;
            }
            //If our claim fails, store held items again
            else
            {
                StoreHeldItems(parent);
            }
        }

    }
}
