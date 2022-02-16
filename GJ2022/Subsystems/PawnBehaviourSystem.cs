using GJ2022.Entities.Pawns;
using GJ2022.Managers;
using GJ2022.PawnBehaviours;
using GLFW;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GJ2022.Subsystems
{
    public class PawnBehaviourSystem : Subsystem
    {

        public static PawnBehaviourSystem Singleton { get; } = new PawnBehaviourSystem();

        //2 second delay between AI fires
        public override int sleepDelay => Math.Min(TimeManager.TimeMultiplier > 0 ? (int)(2000 / TimeManager.TimeMultiplier) : 2000, 2000);

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_PROCESSING;

        public volatile List<PawnBehaviour> processingBehaviours = new List<PawnBehaviour>();

        public override void Fire(Window window)
        {
            if (TimeManager.TimeMultiplier == 0)
                return;
            Log.WriteLine("Calculating AI actions");
            Parallel.ForEach(processingBehaviours, new ParallelOptions { MaxDegreeOfParallelism = 10 },
                behaviour =>
                {
                    behaviour.HandlePawnBehaviour();
                });
            Log.WriteLine("Completed!");
        }

        public void ApplyPawnBehaviour(Pawn target, PawnBehaviour behaviour)
        {
            if (target.behaviourController != null)
                processingBehaviours.Remove(target.behaviourController);
            target.behaviourController = behaviour;
            if (behaviour != null)
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
