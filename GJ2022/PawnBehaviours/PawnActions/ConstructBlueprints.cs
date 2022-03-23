using GJ2022.Components.Bespoke;
using GJ2022.Entities;
using GJ2022.Game.Construction;
using GJ2022.Game.GameWorld;
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
            if (parent.Owner.InCrit)
                return false;
            //Quick check
            return BlueprintSystem.Singleton.HasBlueprints();
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
            Entity target = LocateValidBlueprint(parent);
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
            Entity target = ThreadSafeClaimManager.GetClaimedItem(parent.Owner);
            //Something happened to our target
            if (target == null || !parent.Owner.InReach(target))
            {
                completed = true;
                return;
            }
            //Fetch the associated blueprint data
            Component_Blueprint blueprintComponent = target.GetComponent<Component_Blueprint>();
            //We are at our target
            if (BlueprintSystem.Singleton.BlueprintHasMaterials(blueprintComponent))
                BlueprintSystem.Singleton.CompleteBlueprint(blueprintComponent);
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
        private Entity LocateValidBlueprint(PawnBehaviour parent)
        {
            //Locate the closets blueprint that is not claimed and has sufficient materials
            Entity closestBlueprint = World.GetClosestThing<Entity>("blueprint", (int)parent.Owner.WorldX, (int)parent.Owner.WorldY, 50,
                entity => {
                    return !entity.IsClaimed && BlueprintSystem.Singleton.BlueprintHasMaterials(entity.GetComponent<Component_Blueprint>());
                });

            //If we located a valid blueprint, return it
            if (closestBlueprint != null)
                return closestBlueprint;

            //Pause the action for some time
            parent.PauseActionFor(this, 20);
            //Complete it
            completed = true;
            return null;
        }

    }
}
