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
        }

        public enum SignalResponse
        {
            NONE,
        }

        private static Dictionary<object, Dictionary<Signal, SignalDelegate>> registeredSignals = new Dictionary<object, Dictionary<Signal, SignalDelegate>>();

        public static SignalResponse SendSignal(object target, Signal signal, params object[] data)
        {
            if (registeredSignals.ContainsKey(target) && registeredSignals[target].ContainsKey(signal))
                return registeredSignals[target][signal].Invoke(target, data);
            return SignalResponse.NONE;
        }

        public static void UnregisterSignal(object target, Signal signal)
        {
            registeredSignals[target].Remove(signal);
            if (registeredSignals[target].Count == 0)
                registeredSignals.Remove(target);
        }

        public static void RegisterSignal(object target, Signal signal, SignalDelegate callback)
        {
            if (!registeredSignals.ContainsKey(target))
                registeredSignals.Add(target, new Dictionary<Signal, SignalDelegate>());
            registeredSignals[target].Add(signal, callback);
        }

    }
}
