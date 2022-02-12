using GJ2022.Managers.TaskManager;
using GJ2022.Subsystems;
using System;

namespace GJ2022.Entities.Pawns
{
    public partial class Pawn
    {

        //Are we dead?
        public bool Dead { get; private set; } = false;

        //Are we in crit?
        public bool InCrit { get; private set; } = false;

        public void EnterCrit()
        {
            if (Dead)
                return;
            //Are we in crit already
            if (InCrit)
                return;
            //Enter crit
            InCrit = true;
            //Cancel actions
            behaviourController.PawnActionIntercept(null);
        }

        public void ExitCrit()
        {
            if (Dead)
                return;
            if (!InCrit)
                return;
            //Exit critical condition
            InCrit = false;
            //Reset rotation
            Renderable.UpdateRotation(0);
        }

        /// <summary>
        /// Kill the pawn.
        /// Unclaim things, you are dead and you don't need them anymore
        /// </summary>
        public void Death(string cause)
        {
            Dead = true;
            //Stop procesing
            PawnControllerSystem.Singleton.StopProcessing(this);
            //Rotate
            Renderable.UpdateRotation((float)Math.PI / 2);
            //Stop processing AI
            behaviourController.PawnActionIntercept(null);
            PawnBehaviourSystem.Singleton.ApplyPawnBehaviour(this, null);
            //Unclaim
            ThreadSafeClaimManager.ReleaseClaimBlocking(this);
        }

    }
}
