using GJ2022.Entities.Pawns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.PawnBehaviours
{
    public class PawnBehaviour
    {

        //Memories shared among all pawn behaviours. Acts as a pawn hivemind.
        //The first key represents the memory faction, so pawns that aren't working together won't share.
        public static Dictionary<string, Dictionary<string, object>> SharedMemory { get; } = new Dictionary<string, Dictionary<string, object>>();

        //The memory of this pawn behaviour.
        public Dictionary<string, object> Memory { get; } = new Dictionary<string, object>();

        //Dictionary of pawn actions
        //Value represents the time in which we can perform this action
        //If the value is set to a time in the future, this action has recently failed and
        //will not be performed for some time. (We chose to do this action, but it wasn't possible)
        //(Example: We want to build in space, but cannot move to space due to no space suits, so choose another
        //action for the time being).
        public Dictionary<PawnAction, float> Actions { get; } = new Dictionary<PawnAction, float>();

        //The currently running action
        private PawnAction currentAction;

        //The owner of the pawn behaviour
        public Pawn Owner { get; }

        public PawnBehaviour(Pawn owner)
        {
            Owner = owner;
        }

        /// <summary>
        /// Intercept that forces the pawn to instantly run a new action.
        /// Note that the intercept action can be replaced on the next pawn behaviour.
        /// </summary>
        public void PawnActionIntercept(PawnAction forcedAction)
        {
            if (currentAction != null && !currentAction.Completed(this))
            {
                currentAction.OnActionCancel(this);
            }
            currentAction = forcedAction;
            currentAction.OnActionStart(this);
            currentAction.PerformProcess(this);
        }

        /// <summary>
        /// Handle automatic pawn behaviours
        /// </summary>
        public void HandlePawnBehaviour()
        {
            //Track the highest priority action
            int highestPriorityAction = int.MaxValue;
            PawnAction performingAction = null;
            //Get the actions we can perform
            foreach (PawnAction action in Actions.Keys)
            {
                if (((action.Overriding && !performingAction.Overriding) || action.Priority < highestPriorityAction) && action.CanPerform(this))
                {
                    highestPriorityAction = action.Priority;
                    performingAction = action;
                }
            }
            //If our current action is completed, smoothly swap to the next action
            if (currentAction.Completed(this) && performingAction != null)
            {
                currentAction = performingAction;
                currentAction.OnActionStart(this);
            }
            //Replace current action with overriding actions if available
            else if (performingAction != null && performingAction.Overriding && performingAction != currentAction)
            {
                currentAction.OnActionCancel(this);
                currentAction = performingAction;
                currentAction.OnActionStart(this);
            }
            //Perform current action
            if (currentAction != null)
            {
                currentAction.PerformProcess(this);
            }
        }

    }
}
