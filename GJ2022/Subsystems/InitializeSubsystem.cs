using GJ2022.Entities.ComponentInterfaces;
using System;
using System.Collections.Generic;

namespace GJ2022.Subsystems
{
    public class InitializeSubsystem : Subsystem
    {

        public static InitializeSubsystem Singleton { get; } = new InitializeSubsystem();

        public override int sleepDelay => 0;

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_FIRE | SubsystemFlags.NO_PROCESSING;

        private List<IInitializeBehaviour> initializeQueue = new List<IInitializeBehaviour>();

        public override void Fire()
        {
            throw new NotImplementedException("Initialize SS should not fire.");
        }

        public override void InitSystem() { }

        /// <summary>
        /// Initializes an entity
        /// </summary>
        public void Initialize(IInitializeBehaviour initializeBehaviour)
        {
            if (initializeBehaviour == null)
                return;
            if (initializeQueue == null)
                initializeBehaviour.Initialize();
            else
                initializeQueue.Add(initializeBehaviour);
        }

        protected override void AfterWorldInit()
        {
            //Initialize everything
            foreach (IInitializeBehaviour initializeBehaviour in initializeQueue)
            {
                initializeBehaviour.Initialize();
            }
            //Delete the queue
            initializeQueue = null;
        }

    }
}
