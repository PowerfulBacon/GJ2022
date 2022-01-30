using GJ2022.Managers.TaskManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Managers
{
    public static class SignalHandler
    {

        public delegate SignalResponse SignalDelegate(object source, params object[] data);

        public enum Signal
        {
            SIGNAL_ENTITY_MOVED,
            SIGNAL_ENTITY_DESTROYED,
        }

        public enum SignalResponse
        {
            NONE,
        }

        private static Dictionary<object, Dictionary<Signal, SignalDelegate>> registeredSignals = new Dictionary<object, Dictionary<Signal, SignalDelegate>>();

        public static SignalResponse SendSignalSynchronously(object target, Signal signal, params object[] data)
        {
            SignalResponse result = SignalResponse.NONE;
            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_SIGNALS, () =>
            {
                if (registeredSignals.ContainsKey(target) && registeredSignals[target].ContainsKey(signal))
                    result = registeredSignals[target][signal].Invoke(target, data);
                return true;
            });
            return result;
        }

        public static void SendSignal(object target, Signal signal, params object[] data)
        {
            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_SIGNALS, () =>
            {
                if (registeredSignals.ContainsKey(target) && registeredSignals[target].ContainsKey(signal))
                {
                    SignalDelegate runningDelegate = registeredSignals[target][signal];
                    Task.Run(() => runningDelegate.Invoke(target, data));
                }
                return true;
            });
        }

        public static void UnregisterAll(object target)
        {
            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_SIGNALS, () =>
            {
                if(registeredSignals.ContainsKey(target))
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

        public static void UnregisterSignal(object target, Signal signal)
        {
            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_SIGNALS, () =>
            {
                if (!registeredSignals.ContainsKey(target) || !registeredSignals[target].ContainsKey(signal))
                {
                    Log.WriteLine($"Failed to unregister signal {signal} on {target}, due to it not being registered", LogType.WARNING);
                    return true;
                }
                registeredSignals[target].Remove(signal);
                if (registeredSignals[target].Count == 0)
                    registeredSignals.Remove(target);
                return true;
            });
        }

        public static void RegisterSignal(object target, Signal signal, SignalDelegate callback)
        {
            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_SIGNALS, () =>
            {
                if (!registeredSignals.ContainsKey(target))
                    registeredSignals.Add(target, new Dictionary<Signal, SignalDelegate>());
                registeredSignals[target].Add(signal, callback);
                return true;
            });
        }

    }
}
