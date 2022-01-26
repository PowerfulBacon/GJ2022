using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours;
using GLFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Subsystems
{
    public class PawnBehaviourSystem : Subsystem
    {

        public static PawnBehaviourSystem Singleton { get; } = new PawnBehaviourSystem();

        //2 second delay between AI fires
        public override int sleepDelay => 2000;

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_PROCESSING;

        public volatile List<PawnBehaviour> processingBehaviours = new List<PawnBehaviour>();

        public override void Fire(Window window)
        {
            //Fire the pawn behaviour tasks
            foreach (PawnBehaviour behaviour in processingBehaviours)
            {
                new Task(behaviour.HandlePawnBehaviour).Start();
            }
        }

        public void ApplyPawnBehaviour(Pawn target, PawnBehaviour behaviour)
        {
            if (target.behaviourController != null)
                processingBehaviours.Remove(target.behaviourController);
            target.behaviourController = behaviour;
            processingBehaviours.Add(behaviour);
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
