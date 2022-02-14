using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLFW;

namespace GJ2022.Subsystems.Processing
{
    public class PowerProcessingSystem : Subsystem
    {

        public static PowerProcessingSystem Singleton { get; } = new PowerProcessingSystem();

        public override int sleepDelay => 1000;

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
