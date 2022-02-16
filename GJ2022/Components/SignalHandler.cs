using GJ2022.Managers.TaskManager;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GJ2022.Managers
{
    public static class SignalHandler
    {

        public delegate object SignalDelegate(object source, params object[] data);

        public enum Signal
        {
            SIGNAL_ENTITY_MOVED,
            SIGNAL_ENTITY_DESTROYED,
        }

        /// <summary>
        /// Signals can return a value.
        /// The return value will be the highest priority non null return value from a signal.
        /// 
        /// (object, {(Signal, {(int, {SignalDelegate})})})
        /// </summary>
        private static Dictionary<object, Dictionary<Signal, SortedList<int, List<SignalDelegate>>>> registeredSignals = new Dictionary<object, Dictionary<Signal, SortedList<int, List<SignalDelegate>>>>();

        /// <summary>
        /// Sends a signal to a target, and waits for the response.
        /// </summary>
        /// <returns>The highest-priority non-null signal response object.</returns>
        public static object SendSignalSynchronously(object target, Signal signal, params object[] data)
        {
            object result = null;
            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_SIGNALS, () =>
            {
                if (registeredSignals.ContainsKey(target) && registeredSignals[target].ContainsKey(signal))
                    foreach (int priority in registeredSignals[target][signal].Keys)
                    {
                        foreach (SignalDelegate registeredSignal in registeredSignals[target][signal][priority])
                        {
                            object callResult = registeredSignal.Invoke(target, data);
                            result = result ?? callResult;
                        }
                    }
                return true;
            });
            return result;
        }

        /// <summary>
        /// Sends a signal asynchonously and ignores the response of the signal.
        /// Allows the signal to be sent without blocking execution, however the response of the signal is lost.
        /// </summary>
        public static void SendSignal(object target, Signal signal, params object[] data)
        {
            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_SIGNALS, () =>
            {
                if (registeredSignals.ContainsKey(target) && registeredSignals[target].ContainsKey(signal))
                {
                    foreach (int priority in registeredSignals[target][signal].Keys)
                    {
                        foreach (SignalDelegate registeredSignal in registeredSignals[target][signal][priority])
                        {
                            SignalDelegate runningDelegate = registeredSignal;
                            Task.Run(() => runningDelegate.Invoke(target, data));
                        }
                    }
                }
                return true;
            });
        }

        public static void UnregisterAll(object target)
        {
            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_SIGNALS, () =>
            {
                if (registeredSignals.ContainsKey(target))
                    registeredSignals.Remove(target);
                return true;
            });
        }

        public static bool HasSignals(object target)
        {
            return registeredSignals.ContainsKey(target);
        }

        public static bool HasSignal(object target, Signal signal)
        {
            return registeredSignals.ContainsKey(target) && registeredSignals[target].ContainsKey(signal);
        }

        public static void UnregisterSignal(object target, Signal signal, SignalDelegate callback)
        {
            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_SIGNALS, () =>
            {
                if (!registeredSignals.ContainsKey(target) || !registeredSignals[target].ContainsKey(signal))
                {
                    Log.WriteLine($"Failed to unregister signal {signal} on {target}, due to it not being registered", LogType.WARNING);
                    return true;
                }
                foreach (int priority in registeredSignals[target][signal].Keys)
                {
                    registeredSignals[target][signal][priority].Remove(callback);
                    if (registeredSignals[target][signal][priority].Count == 0)
                        registeredSignals[target][signal].Remove(priority);
                }
                if (registeredSignals[target][signal].Count == 0)
                    registeredSignals[target].Remove(signal);
                if (registeredSignals[target].Count == 0)
                    registeredSignals.Remove(target);
                return true;
            });
        }

        public static void RegisterSignal(object target, Signal signal, int priority, SignalDelegate callback)
        {
            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_SIGNALS, () =>
            {
                if (!registeredSignals.ContainsKey(target))
                    registeredSignals.Add(target, new Dictionary<Signal, SortedList<int, List<SignalDelegate>>>());
                if (!registeredSignals[target].ContainsKey(signal))
                    registeredSignals[target].Add(signal, new SortedList<int, List<SignalDelegate>>());
                if (!registeredSignals[target][signal].ContainsKey(priority))
                    registeredSignals[target][signal].Add(priority, new List<SignalDelegate>());
                registeredSignals[target][signal][priority].Add(callback);
                return true;
            });
        }

    }
}
