using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.PawnBehaviours.PawnActions
{
    //Override action
    public class EscapeHazardousEnvironmentAction : PawnAction
    {

        //Of cousre
        public override bool Overriding => true;

        //Override everything
        public override int Priority => -1;

        //Don't perform this action by default, have it activated as a result of intercepts
        public override bool CanPerform(PawnBehaviour parent)
        {
            return false;
        }

        public override bool Completed(PawnBehaviour parent)
        {
            throw new NotImplementedException();
        }

        public override void OnActionCancel(PawnBehaviour parent)
        {
            throw new NotImplementedException();
        }

        public override void OnActionStart(PawnBehaviour parent)
        {
            throw new NotImplementedException();
        }

        public override void PerformProcess(PawnBehaviour parent)
        {
            throw new NotImplementedException();
        }

    }
}
