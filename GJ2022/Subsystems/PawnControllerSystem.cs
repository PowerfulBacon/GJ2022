using GJ2022.Entities.Abstract;
using GJ2022.Utility.MathConstructs;
using GLFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Subsystems
{
    public class PawnControllerSystem : Subsystem
    {

        public static PawnControllerSystem Singleton { get; } = new PawnControllerSystem();

        public override int sleepDelay => 100;

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_FIRE;

        public Dictionary<Vector, Blueprint[]> QueuedBlueprints { get; } = new Dictionary<Vector, Blueprint[]>();

        public override void Fire(Window window)
        {
            //Ai
        }

        public override void InitSystem()
        { }

        protected override void AfterWorldInit()
        { }

    }
}
