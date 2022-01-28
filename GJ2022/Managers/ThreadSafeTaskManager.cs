using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GJ2022.Managers
{
    public class ThreadSafeTaskManager
    {

        private static object lockObject = new object();

        public const int TASK_PAWN_INVENTORY = 1;
        public const int TASK_PAWN_EQUIPPABLES = 3;
        public const int TASK_STOCKPILE_MANAGER = 2;

        private static volatile bool executing = false;

        private static volatile Dictionary<int, int> totalActionsReserved = new Dictionary<int, int>();
        private static volatile Dictionary<int, int> currentAction = new Dictionary<int, int>();

        /// <summary>
        /// Execute an action in a thread safe manner
        /// </summary>
        /// <param name="threadSafeId">The ID of the thread safe action. Any actions with the same ID will block each other.</param>
        /// <param name="action">The action to perform.</param>
        /// <returns>Returns the result of the action (usually true if the action was successful)</returns>
        public static bool ExecuteThreadSafeAction(int threadSafeId, Func<bool> action)
        {
            int queueId = GetQueueId(threadSafeId);
            int sanity = 0;
            while (!IsReady(queueId, threadSafeId))
            {
                if (sanity++ > 10000)
                {
                    throw new Exception($"Reserve claim ID : {queueId} has been waiting for {sanity} ticks without success. (Current action: {currentAction[threadSafeId]})");
                }
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
                Log.WriteLine(e, LogType.ERROR);
                Complete(threadSafeId);
                throw e;
            }
            //Complete action
            Complete(threadSafeId);
            //We claimed the target
            return result;
        }

        private unsafe static int GetQueueId(int thread_id)
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

        private static bool IsReady(int id, int thread_id)
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
