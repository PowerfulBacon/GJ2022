﻿using GJ2022.Managers.TaskManager;
using GLFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GJ2022.Subsystems
{
    public class TaskManagerSubsystem : Subsystem
    {

        public static TaskManagerSubsystem Singleton { get; } = new TaskManagerSubsystem();

        //We need this to be instant
        public override int sleepDelay => 0;

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_UPDATE;

        private Dictionary<int, Queue<KeyValuePair<int, Func<bool>>>> queuedTasks = new Dictionary<int, Queue<KeyValuePair<int, Func<bool>>>>();

        public void QueueThreadSafeTask(int threadSafeId, Func<bool> action)
        {
            lock (queuedTasks[threadSafeId])
            {
                queuedTasks[threadSafeId].Enqueue(new KeyValuePair<int, Func<bool>>(ThreadSafeTaskManager.GetQueueId(threadSafeId), action));
                //Debug check
                for (int i = 0; i < queuedTasks[threadSafeId].Count; i++)
                {
                    if (queuedTasks[threadSafeId].ElementAt(i).Key < queuedTasks[threadSafeId].ElementAt(0).Key)
                    {
                        Log.WriteLine($"Queued out of order exception: ({i}): {queuedTasks[threadSafeId].ElementAt(i).Key}, (0): {queuedTasks[threadSafeId].ElementAt(0).Key}");
                    }
                }
            }
        }

        public override void Fire(Window window)
        {
        }

        private bool waiting = false;

        public override void InitSystem()
        {
            for (int i = 1; i <= ThreadSafeTaskManager.MAX_TASK_ID; i++)
            {
                queuedTasks.Add(i, new Queue<KeyValuePair<int, Func<bool>>>());
                waiting = true;
                new Thread(() => ProcessingThread(i)).Start();
                while (waiting)
                    continue;
            }

        }

        private void ProcessingThread(int i)
        {
            waiting = false;
            Log.WriteLine($"starting processing thread {i}");
            
            while (Firing)
            {
                try
                {
                    lock (queuedTasks[i])
                    {
                        //Get the queue we are working on
                        Queue<KeyValuePair<int, Func<bool>>> targetQueue = queuedTasks[i];
                        //Check length
                        if (targetQueue.Count == 0)
                            continue;
                        //Pop the first pair
                        KeyValuePair<int, Func<bool>> first = targetQueue.Peek();
                        //Check position in queue
                        if (!ThreadSafeTaskManager.IsReady(first.Key, i))
                            continue;
                        if (i == 8)
                            Log.WriteLine($"hello :) {first.Key} / {targetQueue.Last().Key} (Tasks Left: {targetQueue.Count})");
                        targetQueue.Dequeue();
                        //Act upon it
                        ThreadSafeTaskManager.ExecuteThreadSafeAction(i, first.Value, first.Key);
                    }
                }
                catch (System.Exception e)
                {
                    Log.WriteLine(e, LogType.ERROR);
                }
            }
            
        }

        protected override void AfterWorldInit()
        {
            return;
        }
    }
}
