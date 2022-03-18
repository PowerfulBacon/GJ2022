using GJ2022.Entities.Markers;
using GJ2022.Game.GameWorld;
using GJ2022.Managers.TaskManager;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.PawnBehaviours.PawnActions
{
    public class MineAction : PawnAction
    {

        private MiningMarker forcedTarget;

        public MineAction() : base() { }

        public MineAction(MiningMarker target) : base()
        {
            forcedTarget = target;
        }

        public override bool Overriding => false;

        public override int Priority => 17;

        private HashSet<Vector<float>> unreachableLocations = new HashSet<Vector<float>>();

        private bool completed = false;

        public override bool CanPerform(PawnBehaviour parent)
        {
            if (parent.Owner.InCrit)
                return false;
            //Quick check
            return World.Current.HasMarkerInRange((int)parent.Owner.Position.X, (int)parent.Owner.Position.Y, 60, (Marker toCheck) =>
            {
                return !unreachableLocations.Contains(toCheck.Position);
            });
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
            MiningMarker target = LocateValidMarker(parent);
            //No target was found, or our target couldn't be claimed
            if (target == null || !ThreadSafeClaimManager.ReserveClaimBlocking(parent.Owner, target))
            {
                completed = true;
                return;
            }
            //Go towards the blueprint
            Vector<float>? targetPosition = World.Current.GetFreeAdjacentLocation((int)target.Position.X, (int)target.Position.Y);
            if (targetPosition != null)
                parent.Owner.MoveTowardsPosition(targetPosition.Value);
            else
            {
                //Well, we can't reach that place
                unreachableLocations.Add(target.Position);
                //Oh well, we did it
                completed = true;
            }
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
            MiningMarker target = ThreadSafeClaimManager.GetClaimedItem(parent.Owner) as MiningMarker;
            //Something happened to our target
            if (target == null || !parent.Owner.InReach(target, 1.5f))
            {
                completed = true;
                return;
            }
            //We are at our target
            target.HandleAction(parent.Owner);
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
        private MiningMarker LocateValidMarker(PawnBehaviour parent)
        {
            foreach (Marker marker in World.Current.GetSprialMarkers((int)parent.Owner.Position.X, (int)parent.Owner.Position.Y, 60))
            {
                if (!(marker is MiningMarker))
                    continue;
                //If the marker is unreachable continue
                if (unreachableLocations.Contains(marker.Position))
                    continue;
                //If the marker is already claimed continue
                if (marker.IsClaimed || marker.Destroyed)
                    continue;
                //Is the marker completed surrounded?
                if (World.Current.IsLocationFullyEnclosed((int)marker.Position.X, (int)marker.Position.Y))
                    continue;
                //Return the marker
                return marker as MiningMarker;
            }
            //Pause the action for some time
            parent.PauseActionFor(this, 30);
            //Complete it
            completed = true;
            return null;
        }

    }
}
