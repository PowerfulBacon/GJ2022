using GJ2022.Entities.Pawns;
using GJ2022.Managers;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GJ2022.PawnBehaviours
{
    public abstract class PawnBehaviour
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
        public abstract Dictionary<PawnAction, double> Actions { get; }

        //The currently running action
        public PawnAction CurrentAction { get; private set; }

        //Are we processing?
        private volatile bool firing = false;

        //The owner of the pawn behaviour
        public Pawn Owner { get; }

        //The behavioural faction of this pawn behaviour
        public string Faction { get; } = "crew";

        public PawnBehaviour(Pawn owner)
        {
            Owner = owner;
            PawnBehaviourSystem.Singleton.ApplyPawnBehaviour(owner, this);
        }

        public T GetMemory<T>(string key)
            where T : class
        {
            if (Memory.ContainsKey(key))
                return (T)Memory[key];
            return null;
        }

        /// <summary>
        /// Action reached
        /// </summary>
        public void PawnActionReached()
        {
            CurrentAction.OnPawnReachedLocation(this);
        }

        /// <summary>
        /// The action is unreachable
        /// </summary>
        public void PawnActionUnreachable(Vector<float> unreachableLocation)
        {
            CurrentAction.OnActionUnreachable(this, unreachableLocation);
        }

        /// <summary>
        /// Intercept that forces the pawn to instantly run a new action.
        /// Note that the intercept action can be replaced on the next pawn behaviour.
        /// </summary>
        public void PawnActionIntercept(PawnAction forcedAction)
        {
            if (CurrentAction != null && !CurrentAction.Completed(this))
            {
                CurrentAction.OnActionCancel(this);
            }
            CurrentAction = forcedAction;
            CurrentAction?.OnActionStart(this);
            CurrentAction?.PerformProcess(this);
        }

        public void PauseActionFor(PawnAction action, double time)
        {
            if (Actions.ContainsKey(action))
                Actions[action] = Math.Max(Actions[action], TimeManager.Time + time);
        }

        /// <summary>
        /// Handle automatic pawn behaviours
        /// </summary>
        public void HandlePawnBehaviour()
        {
            if (firing)
                return;
            firing = true;
            try
            {
                //Track the highest priority action
                int highestPriorityAction = int.MaxValue;
                PawnAction performingAction = null;
                //Get the actions we can perform
                //Copy the keys
                foreach (PawnAction action in Actions.Keys.ToList())
                {
                    //Dictionary was modified
                    if (!Actions.ContainsKey(action))
                        continue;
                    double availableTime = Actions[action];
                    if (((action.Overriding && !performingAction.Overriding) || action.Priority < highestPriorityAction) && action.CanPerform(this) && availableTime < TimeManager.Time)
                    {
                        highestPriorityAction = action.Priority;
                        performingAction = action;
                    }
                }
                //If our current action is completed, smoothly swap to the next action
                if ((CurrentAction == null || CurrentAction.Completed(this) || (CurrentAction.ForceOverride && performingAction != CurrentAction)) && performingAction != null)
                {
                    CurrentAction?.OnActionEnd(this);
                    CurrentAction = performingAction;
                    CurrentAction.OnActionStart(this);
                }
                //Replace current action with overriding actions if available
                else if (performingAction != null && performingAction.Overriding && performingAction != CurrentAction)
                {
                    CurrentAction?.OnActionCancel(this);
                    CurrentAction = performingAction;
                    CurrentAction.OnActionStart(this);
                }
                //Perform current action
                if (CurrentAction != null)
                {
                    CurrentAction.PerformProcess(this);
                }
                firing = false;
            }
            catch (Exception e)
            {
                firing = false;
                Log.WriteLine(e, LogType.ERROR);
                throw e;
            }
        }

    }
}
