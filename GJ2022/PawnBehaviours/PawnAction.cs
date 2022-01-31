using GJ2022.Utility.MathConstructs;

namespace GJ2022.PawnBehaviours
{
    public abstract class PawnAction
    {

        //Does this action override existing actions? (If not, lower priority actions will be performed)
        public abstract bool Overriding { get; }

        public virtual bool ForceOverride { get; } = false;

        //The priority of this action
        public abstract int Priority { get; }

        //If true, this action can be performed, otherwise it will be skipped
        public abstract bool CanPerform(PawnBehaviour parent);

        //If true, this action is considered completed and will be swapped with another as soon as another is available.
        public abstract bool Completed(PawnBehaviour parent);

        //Called when the action first starts
        public abstract void OnActionStart(PawnBehaviour parent);

        //Called every process while this action is running including on the first tick (when start is called)
        public abstract void PerformProcess(PawnBehaviour parent);

        //Action was cancelled by a more important action
        public abstract void OnActionCancel(PawnBehaviour parent);

        //Action is unreachable
        public abstract void OnActionUnreachable(PawnBehaviour parent, Vector<float> unreachableLocation);

        //Action location has been reached
        public abstract void OnPawnReachedLocation(PawnBehaviour parent);

        //Called when the action ends, either through completion or cancellation.
        public abstract void OnActionEnd(PawnBehaviour parent);

    }
}
