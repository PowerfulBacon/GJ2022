using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GJ2022.Components.ComponentHandler;

namespace GJ2022.Components
{
    public interface IComponentHandler
    {

        void AddComponent(Component component);

        /// <summary>
        /// Sends a signal to a target, and waits for the response.
        /// </summary>
        /// <returns>The highest-priority non-null signal response object.</returns>
        object SendSignalSynchronously(Signal signal, params object[] data);

        /// <summary>
        /// Sends a signal asynchonously and ignores the response of the signal.
        /// Allows the signal to be sent without blocking execution, however the response of the signal is lost.
        /// </summary>
        void SendSignal(Signal signal, params object[] data);

        /// <summary>
        /// Unregisters all signals attached to this object.
        /// </summary>
        void UnregisterAllSignals();

        /// <summary>
        /// Unregister the provided signal from this object.
        /// This requires that a callback be a reference to a method
        /// rather than a lambda function.
        /// </summary>
        /// <param name="signal">The signal to stop registering.</param>
        /// <param name="callback">The provided method to stop listening.</param>
        void UnregisterSignal(Signal signal, SignalDelegate callback);

        /// <summary>
        /// Register a signal to this object.
        /// Whenever the registered signal is sent to this object,
        /// the provided callback will be executed.
        /// If that callback returns a value, the value returned to the caller
        /// of SendSignal will be the return value of highest priority signal that doesn't
        /// return null.
        /// </summary>
        /// <param name="signal">The identifier of the signal to register</param>
        /// <param name="priority">The priority of the return value of this signal. Use -1 if the callback doesn't return a value.</param>
        /// <param name="callback">The callback to execute when this object recieves the provided signal.</param>
        void RegisterSignal(Signal signal, int priority, SignalDelegate callback);

        void SetProperty(string name, object property);

        void Initialize(Vector<float> initializePosition);

        /// <summary>
        /// Gets the component with the specified type.
        /// Time complexity is proportional to the number of components on the object.
        /// </summary>
        /// <typeparam name="T">The type of the component to return</typeparam>
        /// <returns>The component with type T</returns>
        T GetComponent<T>() where T : Component;

    }
}
