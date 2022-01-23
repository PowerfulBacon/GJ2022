using GJ2022.Utility.MathConstructs;
using GLFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Subsystems
{
    public class PathfindingSystem : Subsystem
    {

        //Little delay
        public override int sleepDelay => 20;

        //No processing, but fires
        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_PROCESSING;

        public override void Fire(Window window)
        {
            throw new NotImplementedException();
        }

        public override void InitSystem()
        { }

        protected override void AfterWorldInit()
        { }

        private void ProcessPath(Vector start, Vector end)
        {

        }

        private void 

    }
}
