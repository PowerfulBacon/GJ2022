﻿using GJ2022.Entities.Pawns;
using GJ2022.Subsystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private PawnAction currentAction;

        //Are we processing?
        private volatile bool firing = false;

        //The owner of the pawn behaviour
        public Pawn Owner { get; }

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
            currentAction.OnPawnReachedLocation(this);
        }

        /// <summary>
        /// The action is unreachable
        /// </summary>
        public void PawnActionUnreachable()
        {
            currentAction.OnActionUnreachable(this);
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

        public void PauseActionFor(PawnAction action, double time)
        {
            if (Actions.ContainsKey(action))
                Actions[action] = Math.Max(Actions[action], GLFW.Glfw.Time + time);
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
                foreach (PawnAction action in Actions.Keys)
                {
                    if (((action.Overriding && !performingAction.Overriding) || action.Priority < highestPriorityAction) && action.CanPerform(this) && Actions[action] < GLFW.Glfw.Time)
                    {
                        highestPriorityAction = action.Priority;
                        performingAction = action;
                    }
                }
                //If our current action is completed, smoothly swap to the next action
                if ((currentAction == null || currentAction.Completed(this)) && performingAction != null)
                {
                    currentAction?.OnActionEnd(this);
                    currentAction = performingAction;
                    currentAction.OnActionStart(this);
                    Log.WriteLine($"Starting new action: {currentAction}");
                }
                //Replace current action with overriding actions if available
                else if (performingAction != null && performingAction.Overriding && performingAction != currentAction)
                {
                    currentAction?.OnActionCancel(this);
                    currentAction = performingAction;
                    currentAction.OnActionStart(this);
                    Log.WriteLine($"Overriding new action: {currentAction}");
                }
                //Perform current action
                if (currentAction != null)
                {
                    currentAction.PerformProcess(this);
                }
                firing = false;
            }
            catch(Exception e)
            {
                firing = false;
                throw e;
            }
        }

    }
}
