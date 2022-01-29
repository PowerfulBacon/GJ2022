﻿using GJ2022.Entities;
using GJ2022.Entities.Items;
using GJ2022.Entities.Items.Stacks.Ores;
using GJ2022.Entities.Structures;
using GJ2022.Game.GameWorld;
using GJ2022.Managers;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.PawnBehaviours.PawnActions
{
    public class SmeltOre : PawnAction
    {

        private enum HaulItemActionModes
        {
            FETCH_ITEM,
            DELIVER_ITEM
        }

        public override bool Overriding => false;

        public override int Priority => 16;

        private bool completed = false;

        private HashSet<Vector<float>> unreachableLocations = new HashSet<Vector<float>>();

        //The current state
        private HaulItemActionModes haulActionState = HaulItemActionModes.FETCH_ITEM;

        public override bool CanPerform(PawnBehaviour parent)
        {
            bool hasFurnace = false;
            foreach (Structure structure in World.GetSprialStructure((int)parent.Owner.Position[0], (int)parent.Owner.Position[1], 20))
            {
                if (structure is Furnace)
                {
                    hasFurnace = true;
                    break;
                }
            }
            if (hasFurnace)
            {
                foreach (Item item in World.GetSprialItems((int)parent.Owner.Position[0], (int)parent.Owner.Position[1], 40))
                {
                    //Todo make this ore
                    if (item is IronOre)
                        return true;
                }
            }
            parent.PauseActionFor(this, 10);
            return false;
        }

        public override bool Completed(PawnBehaviour parent)
        {
            return completed;
        }

        public override void OnActionCancel(PawnBehaviour parent)
        {
            return;
        }

        public override void OnActionEnd(PawnBehaviour parent)
        {
            ReleaseFurnaceClaiedOre(parent);
            ThreadSafeClaimManager.ReleaseClaimBlocking(parent.Owner);
            haulActionState = HaulItemActionModes.FETCH_ITEM;
            completed = false;
        }

        public override void OnActionStart(PawnBehaviour parent)
        {
            ReleaseFurnaceClaiedOre(parent);
            Furnace located = LocateValidFurnace(parent);
            //If we couldn't locate a furnace, or claim a blueprint
            if (located == null || !ThreadSafeClaimManager.ReserveClaimBlocking(parent.Owner, located))
            {
                completed = true;
                return;
            }
            //Locate materials
            IronOre locatedOre = LocateMaterials(parent);
            //If we couldn't locate an item, return
            //Have the furnace reserve the item
            if (locatedOre == null || !ThreadSafeClaimManager.ReserveClaimBlocking(located, locatedOre))
            {
                //We couldn't build the blueprint, so lets ignore it
                unreachableLocations.Add(located.Position);
                completed = true;
                return;
            }
            //Move towaards the material
            parent.Owner.MoveTowardsEntity(locatedOre);
            haulActionState = HaulItemActionModes.FETCH_ITEM;
        }

        public override void OnActionUnreachable(PawnBehaviour parent, Vector<float> unreachableLocation)
        {
            //Drop the items we were holding
            parent.Owner.DropHeldItems(parent.Owner.Position);
            //Mark location as unreachable
            unreachableLocations.Add(unreachableLocation);
            //Complete the task
            completed = true;
        }

        public override void OnPawnReachedLocation(PawnBehaviour parent)
        {
            Furnace targetFurnace = ThreadSafeClaimManager.GetClaimedItem(parent.Owner) as Furnace;
            switch (haulActionState)
            {
                case HaulItemActionModes.DELIVER_ITEM:
                    //If we have no target blueprint, drop items and do something else
                    if (targetFurnace == null)
                    {
                        //Fail
                        parent.Owner.DropHeldItems(parent.Owner.Position);
                        completed = true;
                        return;
                    }
                    //Get items we are holding and put them into the blueprint
                    //Put the item inside the blueprint
                    foreach (Item item in parent.Owner.GetHeldItems())
                    {
                        if (!(item is IronOre))
                            continue;
                        ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_PAWN_INVENTORY, () =>
                        {
                            targetFurnace.Smelt(parent.Owner, item as IronOre);
                            return true;
                        });
                    }
                    completed = true;
                    return;
                case HaulItemActionModes.FETCH_ITEM:
                    //We have no claimed blueprint
                    if (targetFurnace == null)
                    {
                        completed = true;
                        return;
                    }
                    Item targetItem = ThreadSafeClaimManager.GetClaimedItem(targetFurnace) as Item;
                    //If we have no target item, or we cannot pickup the target item then complete the task
                    if (targetItem == null || targetItem.Destroyed || !targetItem.InReach(parent.Owner))
                    {
                        completed = true;
                        return;
                    }
                    //Pickup the target item
                    if (!parent.Owner.TryPickupItem(targetItem))
                    {
                        completed = true;
                        return;
                    }
                    //Move towards the target blueprint
                    parent.Owner.MoveTowardsEntity(targetFurnace);
                    haulActionState = HaulItemActionModes.DELIVER_ITEM;
                    return;
            }
        }

        public override void PerformProcess(PawnBehaviour parent)
        {
            return;
        }

        private Furnace LocateValidFurnace(PawnBehaviour parent)
        {
            foreach (Structure structure in World.GetSprialStructure((int)parent.Owner.Position[0], (int)parent.Owner.Position[1], 20))
            {
                //If we marked this location as unreachable, ignore it.
                if (unreachableLocations.Contains(structure.Position))
                    continue;
                //Not a furnace
                if (!(structure is Furnace))
                    continue;
                //Claimed
                if (structure.IsClaimed)
                    continue;
                return structure as Furnace;
            }
            //Nope
            parent.PauseActionFor(this, 10);
            unreachableLocations.Clear();
            completed = true;
            return null;
        }

        /// <summary>
        /// Find required materials in the stockpile
        /// </summary>
        private IronOre LocateMaterials(PawnBehaviour parent)
        {
            foreach (Item item in World.GetSprialItems((int)parent.Owner.Position[0], (int)parent.Owner.Position[1], 40))
            {
                //If we marked this location as unreachable, ignore it.
                if (unreachableLocations.Contains(item.Position))
                    continue;
                //Not a furnace
                if (!(item is IronOre))
                    continue;
                //Claimed
                if (item.IsClaimed)
                    continue;
                return item as IronOre;
            }
            //Nope
            parent.PauseActionFor(this, 10);
            unreachableLocations.Clear();
            completed = true;
            return null;
        }

        private void ReleaseFurnaceClaiedOre(PawnBehaviour parent)
        {
            Entity claimed = ThreadSafeClaimManager.GetClaimedItem(parent.Owner);
            if (claimed != null)
            {
                //Release the blueprints item claims
                ThreadSafeClaimManager.ReleaseClaimBlocking(claimed);
            }
        }

    }
}
