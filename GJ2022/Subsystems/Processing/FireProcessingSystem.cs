using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLFW;

namespace GJ2022.Subsystems.Processing
{
    public class FireProcessingSystem : Subsystem
    {

        public static FireProcessingSystem Singleton { get; } = new FireProcessingSystem();

        public override int sleepDelay => 500;

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_FIRE;

        public override void Fire(Window window)
        {
            throw new NotImplementedException();
        }

        public override void InitSystem()
        {
            return;
        }

        protected override void AfterWorldInit()
        {
            return;
        }
    }
}
