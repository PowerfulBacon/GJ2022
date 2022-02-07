using GJ2022.Entities;
using GJ2022.Entities.Blueprints;
using GJ2022.Entities.Items;
using GJ2022.Managers.Stockpile;
using GJ2022.Managers.TaskManager;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;

namespace GJ2022.PawnBehaviours.PawnActions
{
    /// <summary>
    /// Deliver materials to a blueprint.
    /// Due to each entity only being able to claim 1 item,
    /// the pawn claims the blueprint and the blueprint claims the item.
    /// </summary>
    public class DeliverMaterialsToBlueprints : PawnAction
    {

        private enum HaulItemActionModes
        {
            FETCH_ITEM,
            DELIVER_ITEM
        }

        public override bool Overriding => false;

        public override int Priority => 19;

        private bool completed = false;

        private HashSet<Vector<float>> unreachableLocations = new HashSet<Vector<float>>();

        //The current state
        private HaulItemActionModes haulActionState = HaulItemActionModes.FETCH_ITEM;

        public override bool CanPerform(PawnBehaviour parent)
        {
            if (parent.Owner.InCrit)
                return false;
            return PawnControllerSystem.QueuedBlueprints.Count > 0;
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
            Entity claimed = ThreadSafeClaimManager.GetClaimedItem(parent.Owner);
            if (claimed != null)
            {
                //Release the blueprints item claims
                ThreadSafeClaimManager.ReleaseClaimBlocking(claimed);
            }
            ThreadSafeClaimManager.ReleaseClaimBlocking(parent.Owner);
            haulActionState = HaulItemActionModes.FETCH_ITEM;
            completed = false;
        }

        public override void OnActionStart(PawnBehaviour parent)
        {
            ReleaseBlueprintClaimedItems(parent);
            Blueprint located = LocateValidBlueprint(parent);
            //If we couldn't locate a blueprint, or claim a blueprint
            if (located == null || !ThreadSafeClaimManager.ReserveClaimBlocking(parent.Owner, located))
            {
                completed = true;
                return;
            }
            //Locate materials
            Item locatedMaterials = LocateMaterials(parent, located);
            //If we couldn't locate an item, return
            //Have the blueprint reserve the item
            if (locatedMaterials == null || !ThreadSafeClaimManager.ReserveClaimBlocking(located, locatedMaterials))
            {
                //We couldn't build the blueprint, so lets ignore it
                unreachableLocations.Add(located.Position);
                completed = true;
                return;
            }
            //Move towaards the material
            parent.Owner.MoveTowardsEntity(locatedMaterials);
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
            Blueprint targetBlueprint = ThreadSafeClaimManager.GetClaimedItem(parent.Owner) as Blueprint;
            switch (haulActionState)
            {
                case HaulItemActionModes.DELIVER_ITEM:
                    //If we have no target blueprint, drop items and do something else
                    if (targetBlueprint == null)
                    {
                        //Fail
                        parent.Owner.DropHeldItems(parent.Owner.Position);
                        completed = true;
                        return;
                    }
                    //Get items we are holding and put them into the blueprint
                    //Put the item inside the blueprint
                    Type firstType = null;
                    foreach (Item item in parent.Owner.GetHeldItems())
                    {
                        firstType = item.GetType();
                        if (targetBlueprint.RequiresMaterial(item.GetType()))
                        {
                            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_PAWN_INVENTORY, () =>
                            {
                                targetBlueprint.PutMaterials(item);
                                return true;
                            });
                        }
                    }
                    //Attempt to find more blueprints like this
                    LocateSimilarBlueprints(parent, firstType);
                    return;
                case HaulItemActionModes.FETCH_ITEM:
                    //We have no claimed blueprint
                    if (targetBlueprint == null)
                    {
                        completed = true;
                        return;
                    }
                    Item targetItem = ThreadSafeClaimManager.GetClaimedItem(targetBlueprint) as Item;
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
                    parent.Owner.MoveTowardsEntity(targetBlueprint);
                    haulActionState = HaulItemActionModes.DELIVER_ITEM;
                    return;
            }
        }

        public override void PerformProcess(PawnBehaviour parent)
        {
            return;
        }

        private Blueprint LocateValidBlueprint(PawnBehaviour parent)
        {
            foreach (Vector<float> blueprintPosition in PawnControllerSystem.QueuedBlueprints.Keys)
            {
                //If we marked this location as unreachable, ignore it.
                if (unreachableLocations.Contains(blueprintPosition))
                    continue;
                //Queued blueprints
                Dictionary<int, Blueprint> queuedBlueprintsAtLocation = PawnControllerSystem.QueuedBlueprints[blueprintPosition];
                //Find blueprints with materials
                foreach (Blueprint blueprint in queuedBlueprintsAtLocation.Values)
                {
                    //Blueprint is already claimed
                    if (blueprint.IsClaimed)
                        continue;
                    //Check if there are materials in storage, and the blueprint requires materials
                    (Type, int)? requiredMaterial = blueprint.GetRequiredMaterial();
                    if (requiredMaterial == null || StockpileManager.LocateItemInStockpile(requiredMaterial.Value.Item1) == null)
                        continue;
                    //If the blueprint has materials, then we can build it.
                    return blueprint;
                }
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
        private Item LocateMaterials(PawnBehaviour parent, Blueprint blueprint)
        {
            if (blueprint.GetRequiredMaterial() == null)
                return null;
            Type wantedType = blueprint.GetRequiredMaterial().Value.Item1;
            Item coolItem = StockpileManager.LocateItemInStockpile(wantedType);
            if (coolItem == null)
                return null;
            if (coolItem.IsClaimed)
                return null;
            return coolItem;
        }

        /// <summary>
        /// Attempt to locate blueprints that need the material we have
        /// </summary>
        private void LocateSimilarBlueprints(PawnBehaviour parent, Type desiredMaterialType)
        {
            foreach (Vector<float> blueprintPosition in PawnControllerSystem.QueuedBlueprints.Keys)
            {
                //If we marked this location as unreachable, ignore it.
                if (unreachableLocations.Contains(blueprintPosition))
                    continue;
                //Queued blueprints
                Dictionary<int, Blueprint> queuedBlueprintsAtLocation = PawnControllerSystem.QueuedBlueprints[blueprintPosition];
                //Find blueprints with materials
                foreach (Blueprint blueprint in queuedBlueprintsAtLocation.Values)
                {
                    //Blueprint is already claimed
                    if (blueprint.IsClaimed)
                        continue;
                    //Check if there are materials in storage, and the blueprint requires materials
                    (Type, int)? requiredMaterial = blueprint.GetRequiredMaterial();
                    if (requiredMaterial == null || requiredMaterial?.Item1 != desiredMaterialType)
                        continue;
                    //If we couldn't locate a blueprint, or claim a blueprint
                    ReleaseBlueprintClaimedItems(parent);
                    if (blueprint == null || !ThreadSafeClaimManager.ReserveClaimBlocking(parent.Owner, blueprint))
                        continue;
                    //Move towards it
                    parent.Owner.MoveTowardsEntity(blueprint);
                    haulActionState = HaulItemActionModes.DELIVER_ITEM;
                    return;
                }
            }
            //Nope
            completed = true;
            parent.Owner.DropHeldItems(parent.Owner.Position);
        }

        private void ReleaseBlueprintClaimedItems(PawnBehaviour parent)
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
