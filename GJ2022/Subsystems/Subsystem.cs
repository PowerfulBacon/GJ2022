using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Managers;
using GLFW;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace GJ2022.Subsystems
{

    public enum SubsystemFlags
    {
        //Raw flags
        NONE = 0,
        NO_FIRE = (1 << 1),
        NO_PROCESSING = (1 << 2),
        //Combinations
        NO_UPDATE = NO_FIRE | NO_PROCESSING,
    }

    public abstract class Subsystem
    {

        //Sleep delay in milliseconds
        public abstract int sleepDelay { get; }

        //Flags of the subsystem
        public abstract SubsystemFlags SubsystemFlags { get; }

        //Subsystem control variable
        //If this valie is set to false, the subsystem will be killed
        public static bool Firing { get; protected set; } = true;

        //Is the subsystem started?
        private volatile bool started = false;
        //The time it took between the last frame and the current.
        //Due to only being measured to the millisecond, this can be 0 so watch out with divisions.
        private float deltaTime = 1.0f;

        //A list of things we are processing
        private List<IProcessable> ProcessingEntities { get; } = new List<IProcessable>();

        /// <summary>
        /// Shuts down all subsystems.
        /// </summary>
        public static void KillSubsystems()
        {
            Firing = false;
        }

        private static List<Subsystem> queuedSystems = new List<Subsystem>();

        /// <summary>
        /// Queue the subsystem for intialization
        /// </summary>
        protected Subsystem()
        {
            if (queuedSystems != null)
                queuedSystems.Add(this);
            else
                throw new System.Exception("Subsystem created post subsystem initialization!");
        }

        /// <summary>
        /// Run initialization on all queued subsystems
        /// </summary>
        public static void InitializeSubsystems(Window window)
        {
            foreach (Subsystem ss in queuedSystems)
            {
                ss.Initialize(window);
            }
            queuedSystems = null;
        }

        /// <summary>
        /// Run initialization on the subsystem
        /// </summary>
        private void Initialize(Window window)
        {
            //Log
            Log.WriteLine($"Initializing system {ToString()}", LogType.DEBUG);

            //Initialize
            InitSystem();

            //Create the thread for this system
            //If the subsystem doesn't process or fire, don't start a thread for it.
            if ((SubsystemFlags & SubsystemFlags.NO_UPDATE) != SubsystemFlags.NO_UPDATE)
            {
                Thread thread = new Thread(() => Update(window));
                thread.Name = $"{GetType()} Subsystem Update";
                thread.Start();
            }
        }

        //Method called after the world has been initialized.
        protected abstract void AfterWorldInit();

        /// <summary>
        /// Method gets the singleton of all subsystems, forcing them to be created if they aren't already.
        /// The compiler will optimise this method away since it technically does nothing, but it is actually
        /// quite important so we need to disable optimization for this method.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public static void InitializeSingletons()
        {
            //https://stackoverflow.com/questions/857705/get-all-derived-types-of-a-type
            //Locate all types of subsystem
            Type[] subsystems = (
                from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                    // alternative: from domainAssembly in domainAssembly.GetExportedTypes()
                from assemblyType in domainAssembly.GetTypes()
                where typeof(Subsystem).IsAssignableFrom(assemblyType)
                // alternative: where assemblyType.IsSubclassOf(typeof(B))
                // alternative: && ! assemblyType.IsAbstract
                select assemblyType).ToArray();
            foreach (Type subsystem in subsystems)
            {
                //TODO Add proper network checks onto this
                if (subsystem.IsAbstract)
                    continue;
                PropertyInfo propInfo = subsystem.GetProperty("Singleton");
                if (propInfo == null)
                    continue;
                propInfo.GetValue(null, null);
            }
        }

        public static void WorldInitialize()
        {
            //https://stackoverflow.com/questions/857705/get-all-derived-types-of-a-type
            //Locate all types of subsystem
            Type[] subsystems = (
                from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                    // alternative: from domainAssembly in domainAssembly.GetExportedTypes()
                from assemblyType in domainAssembly.GetTypes()
                where typeof(Subsystem).IsAssignableFrom(assemblyType)
                // alternative: where assemblyType.IsSubclassOf(typeof(B))
                // alternative: && ! assemblyType.IsAbstract
                select assemblyType).ToArray();
            foreach (Type subsystem in subsystems)
            {
                //TODO Add proper network checks onto this
                if (subsystem.IsAbstract)
                    continue;
                PropertyInfo propInfo = subsystem.GetProperty("Singleton");
                if (propInfo == null)
                    continue;
                Subsystem SS = propInfo.GetValue(null, null) as Subsystem;
                if (SS != null)
                {
                    Log.WriteLine($"Triggering after world init of {SS}", LogType.DEBUG);
                    SS.AfterWorldInit();
                    SS.started = true;
                }
            }
        }

        public abstract void InitSystem();

        private Stopwatch stopwatch;

        //Update class.
        //Executed on a seperate thread from the renderer.
        //Will sleep for a set time between each loop.
        //When firing is set to false, the subsystem should shut down.
        private void Update(Window window)
        {
            while (!started && Firing)
            {
                //Sleep to not consume all the CPU
                Thread.Sleep(10);
            }
            while (Firing)
            {
                //Start a stopwatch to get time taken for execution.
                stopwatch = new Stopwatch();
                stopwatch.Start();
                //Check if the world has been initialized yet.
                if (/*World.Current.Initialized*/ true)
                {
                    //Check the subsystem fires.
                    if ((SubsystemFlags & SubsystemFlags.NO_FIRE) == 0)
                    {
                        //Error handling.
                        try
                        {
                            //Fire the subsystem.
                            //This method is overriden by the subclasses. (Polymorphism)
                            Fire(window);
                        }
                        catch (System.Exception e)
                        {
                            //Handle any exceptions that may occur as a result of bugs.
                            //ErrorHandler.HandleError(e, $"Subsystem {GetType().Name} Fire");
                            Log.WriteLine(e, LogType.ERROR);
                        }
                    }
                    //Handle entitity processing
                    //Any entity that uses process() is handled here.
                    try
                    {
                        if ((SubsystemFlags & SubsystemFlags.NO_PROCESSING) != SubsystemFlags.NO_PROCESSING)
                        {
                            //Perform entity processing
                            ProcessEntities();
                        }
                    }
                    catch (System.Exception e)
                    {
                        //Handle errors due to processing bugs.
                        //ErrorHandler.HandleError(e, $"Subsystem {GetType().Name} Process");
                        Log.WriteLine(e, LogType.ERROR);
                    }
                }
                //Subsystem loop is complete, stop the stopwatch
                stopwatch.Stop();
                //Calculate the time it took to perform the tasks and work out how much time we need to sleep for.
                int timeLeftToSleep = (int)(sleepDelay - stopwatch.ElapsedMilliseconds);
                //If we need to sleep, sleep
                if (timeLeftToSleep > 0)
                {
                    //Keep sleeping until we don't need to anymore
                    while (timeLeftToSleep > 0)
                    {
                        //Check if we have been disabled
                        if (!Firing)
                        {
                            Log.WriteLine($"{ToString()} processing stopped.", LogType.MESSAGE);
                            return;
                        }
                        //Sleep for 1 second
                        Thread.Sleep(Math.Min(timeLeftToSleep, 1000));
                        timeLeftToSleep -= 1000;
                    }
                    //Delta time = the sleep delay.
                    deltaTime = sleepDelay / 1000.0f;
                }
                else
                {
                    //Don't wait, fire again immediately
                    deltaTime = stopwatch.ElapsedMilliseconds / 1000.0f;
                }

            }

            Log.WriteLine($"{ToString()} processing stopped.", LogType.MESSAGE);
        }

        /// <summary>
        /// Returns true if the subsystem is overtiming
        /// </summary>
        protected bool IsOvertiming()
        {
            return stopwatch.ElapsedMilliseconds > sleepDelay;
        }

        /// <summary>
        /// Process entities.
        /// Calls the process() method on any entity that requests processing from this subsystem.
        /// Automatically removes destroyed entities, however that should be done by the entities Destroy() anyway.
        /// </summary>
        private void ProcessEntities()
        {
            //Go through all the entities in the list from highest to lowest.
            //Since the list can be modified, we need to count down so that we dont process the same entity twice.
            for (int i = ProcessingEntities.Count - 1; i >= 0; i--)
            {
                //If the entity is destroyed, stop processing.
                if (ProcessingEntities[i].Destroyed)
                {
                    ProcessingEntities.RemoveAt(i);
                }
                else
                {
                    //Process the entity.
                    ProcessingEntities[i].Process(deltaTime * (float)TimeManager.TimeMultiplier);
                }
            }
        }

        /// <summary>
        /// Virtual fire class, handles the actions of the subsystem.
        /// Should be overriden by the subclass if nofire is set to false.
        /// </summary>
        public abstract void Fire(Window window);

        /// <summary>
        /// Adds an entity to the processing queue, so its process() method will be called.
        /// </summary>
        public void StartProcessing(IProcessable e)
        {
            if ((SubsystemFlags & SubsystemFlags.NO_PROCESSING) == SubsystemFlags.NO_PROCESSING)
                throw new System.Exception($"Subsystem {GetType()} does not support processing.");
            ProcessingEntities.Add(e);
        }

        /// <summary>
        /// Stops processing the specified entity.
        /// </summary>
        public void StopProcessing(IProcessable e)
        {
            ProcessingEntities.Remove(e);
        }

        /// <summary>
        /// Gets a report of the subsystem firing time.
        /// </summary>
        public string GetReport()
        {
            if (deltaTime > sleepDelay)
            {
                return $"{ToString()}: !!! {deltaTime}ms !!!";
            }
            return $"{ToString()}: {deltaTime}ms";
        }

    }
}
