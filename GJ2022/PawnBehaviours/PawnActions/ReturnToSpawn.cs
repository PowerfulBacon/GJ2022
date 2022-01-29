using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.PawnBehaviours.PawnActions
{
    class ReturnToSpawn : PawnAction
    {

        private static Random random = new Random();

        public override bool Overriding => false;

        public override int Priority => 0;

        public override bool ForceOverride => true;

        private bool completed = false;

        private bool waitStarted = false;

        public override bool CanPerform(PawnBehaviour parent)
        {
            return parent.Owner.Position.Length() > 20;
        }

        public override bool Completed(PawnBehaviour parent)
        {
            //Always try to get rid of this action
            return completed;
        }

        public override void OnActionCancel(PawnBehaviour parent)
        {
            return;
        }

        public override void OnActionEnd(PawnBehaviour parent)
        {
            if(waitStarted)
                waitStarted = false;
            completed = false;
            return;
        }

        public override void OnActionStart(PawnBehaviour parent)
        {
            if (waitStarted)
                parent.Owner.MoveTowardsPosition(new Vector<float>(random.Next(-10, 10), random.Next(-10, 10)));
            else
            {
                //Wait for 30 seconds before actually returning to spawn
                parent.PauseActionFor(this, 30);
                waitStarted = true;
                completed = true;
            }
        }

        public override void OnActionUnreachable(PawnBehaviour parent, Vector<float> unreachableLocation)
        {
            completed = true;
            return;
        }

        public override void OnPawnReachedLocation(PawnBehaviour parent)
        {
            completed = true;
            return;
        }

        public override void PerformProcess(PawnBehaviour parent)
        {
            return;
        }
    }
}
