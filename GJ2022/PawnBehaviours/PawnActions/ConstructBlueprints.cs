using GJ2022.Entities.Blueprints;
using GJ2022.Managers.TaskManager;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.PawnBehaviours.PawnActions
{
    public class ConstructBlueprints : PawnAction
    {
        public override bool Overriding => false;

        public override int Priority => 18;

        private HashSet<Vector<float>> unreachableLocations = new HashSet<Vector<float>>();

        private bool completed = false;

        public override bool CanPerform(PawnBehaviour parent)
        {
            //Quick check
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
            //Unclaim blueprint
            ThreadSafeClaimManager.ReleaseClaimBlocking(parent.Owner);
            completed = false;
        }

        public override void OnActionStart(PawnBehaviour parent)
        {
            Blueprint target = LocateValidBlueprint(parent);
            //No target was found, or our target couldn't be claimed
            if (target == null || !ThreadSafeClaimManager.ReserveClaimBlocking(parent.Owner, target))
            {
                completed = true;
                return;
            }
            //Go towards the blueprint
            parent.Owner.MoveTowardsEntity(target);
        }

        public override void OnActionUnreachable(PawnBehaviour parent, Vector<float> unreachableLocation)
        {
            //Well, we can't reach that place
            unreachableLocations.Add(unreachableLocation);
            //Oh well, we did it
            completed = true;
        }

        public override void OnPawnReachedLocation(PawnBehaviour parent)
        {
            //Build the blueprint
            Blueprint target = ThreadSafeClaimManager.GetClaimedItem(parent.Owner) as Blueprint;
            //Something happened to our target
            if (target == null || !parent.Owner.InReach(target))
            {
                completed = true;
                return;
            }
            //We are at our target
            if (target.HasMaterials())
                target.Complete();
            completed = true;
        }

        public override void PerformProcess(PawnBehaviour parent)
        {
            return;
        }

        /// <summary>
        /// Intensive check for finding blueprints
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        private Blueprint LocateValidBlueprint(PawnBehaviour parent)
        {
            foreach (Vector<float> blueprintPosition in PawnControllerSystem.QueuedBlueprints.Keys)
            {
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
                    //If the blueprint has materials, then we can build it.
                    if (blueprint.HasMaterials())
                        return blueprint;
                }
            }
            //Pause the action for some time
            parent.PauseActionFor(this, 20);
            //Complete it
            completed = true;
            return null;
        }

    }
}
