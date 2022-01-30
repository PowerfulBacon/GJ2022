using GJ2022.Subsystems;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GJ2022.Managers.TaskManager
{
    public class ThreadSafeTaskManager
    {

        private static object lockObject = new object();

        public const int TASK_PAWN_INVENTORY = 1;
        public const int TASK_PAWN_EQUIPPABLES = 3;
        public const int TASK_STOCKPILE_MANAGER = 2;
        public const int TASK_RENDERING = 4;
        public const int TASK_MOUSE_SYSTEM = 5;
        public const int TASK_SIGNALS = 6;

        public const int MAX_TASK_ID = 6;

        private static volatile Dictionary<int, int> totalActionsReserved = new Dictionary<int, int>();
        private static volatile Dictionary<int, int> currentAction = new Dictionary<int, int>();

        public static void ExecuteThreadSafeActionUnblocking(int threadSafeId, Func<bool> action)
        {
            TaskManagerSubsystem.Singleton.QueueThreadSafeTask(threadSafeId, action);
        }

        /// <summary>
        /// Execute an action in a thread safe manner
        /// </summary>
        /// <param name="threadSafeId">The ID of the thread safe action. Any actions with the same ID will block each other.</param>
        /// <param name="action">The action to perform.</param>
        /// <returns>Returns the result of the action (usually true if the action was successful)</returns>
        public static bool ExecuteThreadSafeAction(int threadSafeId, Func<bool> action, int? overrideQueueId = null)
        {
            int queueId = overrideQueueId ?? GetQueueId(threadSafeId);
            int sanity = 0;
            while (!IsReady(queueId, threadSafeId))
            {
                if (sanity++ > 100000)
                {
                    sanity = 0;
                    Log.WriteLine($"Reserve claim ID : {queueId} has been waiting for >100000 ticks without success. (Current action: {currentAction[threadSafeId]})");
                }
                //Only yield after 1000 tries
                Thread.Yield();
            }
            bool result;
            //Execute task
            try
            {
                //Catch exceptions
                result = action.Invoke();
            }
            catch (Exception e)
            {
                Log.WriteLine($"Error while performing {action.Method.Name} on {action.Method.DeclaringType.FullName}", LogType.ERROR);
                Log.WriteLine(e, LogType.ERROR);
                Complete(threadSafeId);
                throw e;
            }
            //Complete action
            Complete(threadSafeId);
            //We claimed the target
            return result;
        }

        public unsafe static int GetQueueId(int thread_id)
        {
            lock (lockObject)
            {
                if (totalActionsReserved.ContainsKey(thread_id))
                {
                    int numToIncrement = totalActionsReserved[thread_id];
                    totalActionsReserved[thread_id] = Interlocked.Increment(ref numToIncrement);
                    return totalActionsReserved[thread_id];
                }
                else
                {
                    totalActionsReserved.Add(thread_id, 0);
                    currentAction.Add(thread_id, 0);
                    return 0;
                }
            }
        }

        public static bool IsReady(int id, int thread_id)
        {
            if (currentAction[thread_id] > id)
            {
                throw new Exception("Thread lock detected in task manager");
            }
            return id == currentAction[thread_id];
        }

        private static void Complete(int thread_id)
        {
            currentAction[thread_id]++;
        }

    }
}
