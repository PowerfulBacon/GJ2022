using GJ2022.EntityLoading;
using GJ2022.EntityLoading.XmlDataStructures;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GJ2022.Components
{
    public abstract class ComponentHandler : IComponentHandler, IInstantiatable
    {

        //The attached type definiton, provided during instantiation
        public EntityDef TypeDef { get; set; }

        /// <summary>
        /// The signal delegate.
        /// </summary>
        /// <param name="source">The object the signal was called on</param>
        /// <param name="data">Any data provided</param>
        /// <returns></returns>
        public delegate object SignalDelegate(object source, params object[] data);

        /// <summary>
        /// A hashset containing all components attached to this.
        /// Use AddComponent, do not directly add to this dictionary.
        /// ====Every entity having a dictionary is memory expensive====
        /// </summary>
        private List<Component> Components { get; } = new List<Component>();

        /// <summary>
        /// Adds a component to a component handler.
        /// Sets the parent value of the componet,
        /// starts tracking the component
        /// and calls OnComponentAdd()
        /// Note that any properties that are set up by
        /// XML entity files need to be set before AddComponent is
        /// called.
        /// </summary>
        /// <param name="component">The component to add</param>
        public void AddComponent(Component component)
        {
            //Set the parent
            component.Attach(this);
            //Add the component
            Components.Add(component);
            //Call on component add
            component.OnComponentAdd();
        }

        public void RemoveComponent(Component component)
        {
            if (!Components.Contains(component))
                return;
            //Call onComponentRemove
            component.OnComponentRemove();
            //Remove the component
            Components.Remove(component);
            //Detach from the parent
            component.Attach(null);
        }

        public void RemoveAllComponents()
        {
            for(int i = Components.Count - 1; i >= 0; i--)
                RemoveComponent(Components[i]);
        }

        /// <summary>
        /// A dictionary containing:
        /// Key: The identifier for a signal
        /// Value: A sorted list, sorted by priority containing a list that contains callbacks to run for that signal
        /// </summary>
        private Dictionary<Signal, SortedList<int, List<SignalDelegate>>> registeredSignals = new Dictionary<Signal, SortedList<int, List<SignalDelegate>>>();

        /// <summary>
        /// Sends a signal to a target, and waits for the response.
        /// </summary>
        /// <returns>The highest-priority non-null signal response object.</returns>
        public object SendSignalSynchronously(Signal signal, params object[] data)
        {
            object result = null;
            lock(registeredSignals)
            {
                if (registeredSignals.ContainsKey(signal))
                    foreach (int priority in registeredSignals[signal].Keys)
                    {
                        foreach (SignalDelegate registeredSignal in registeredSignals[signal][priority])
                        {
                            object callResult = registeredSignal.Invoke(this, data);
                            result = result ?? callResult;
                        }
                    }
            }
            return result;
        }

        /// <summary>
        /// Sends a signal asynchonously and ignores the response of the signal.
        /// Allows the signal to be sent without blocking execution, however the response of the signal is lost.
        /// </summary>
        public void SendSignal(Signal signal, params object[] data)
        {
            lock (registeredSignals)
            {
                if (registeredSignals.ContainsKey(signal))
                {
                    foreach (int priority in registeredSignals[signal].Keys)
                    {
                        foreach (SignalDelegate registeredSignal in registeredSignals[signal][priority])
                        {
                            SignalDelegate runningDelegate = registeredSignal;
                            Task.Run(() => runningDelegate.Invoke(this, data));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Unregisters all signals attached to this object.
        /// </summary>
        public void UnregisterAllSignals()
        {
            lock (registeredSignals)
            {
                registeredSignals.Clear();
            }
        }

        /// <summary>
        /// Unregister the provided signal from this object.
        /// This requires that a callback be a reference to a method
        /// rather than a lambda function.
        /// </summary>
        /// <param name="signal">The signal to stop registering.</param>
        /// <param name="callback">The provided method to stop listening.</param>
        public void UnregisterSignal(Signal signal, SignalDelegate callback)
        {
            lock(registeredSignals)
            {
                if (!registeredSignals.ContainsKey(signal))
                    return;
                for (int i = registeredSignals[signal].Count - 1; i >= 0; i--)
                {
                    int priority = registeredSignals[signal].Keys.ElementAt(i);
                    registeredSignals[signal][priority].Remove(callback);
                    if (registeredSignals[signal][priority].Count == 0)
                        registeredSignals[signal].Remove(priority);
                }
                if (registeredSignals[signal].Count == 0)
                    registeredSignals.Remove(signal);
                return;
            }
        }

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
        public void RegisterSignal(Signal signal, int priority, SignalDelegate callback)
        {
            lock(registeredSignals)
            {
                if (!registeredSignals.ContainsKey(signal))
                    registeredSignals.Add(signal, new SortedList<int, List<SignalDelegate>>());
                if (!registeredSignals[signal].ContainsKey(priority))
                    registeredSignals[signal].Add(priority, new List<SignalDelegate>());
                registeredSignals[signal][priority].Add(callback);
            }
        }

        public virtual void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "Components":
                    PropertyDef componentProperty = (PropertyDef)property;
                    foreach (PropertyDef component in componentProperty.GetChildren())
                    {
                        try
                        {
                            EntityDef componentEntity = (EntityDef)component;
                            Component instantiatedComponent = (Component)componentEntity.GetValue(Vector<float>.Zero, true);
                            AddComponent(instantiatedComponent);
                        }
                        catch (XmlException e)
                        {
                            Log.WriteLine(e, LogType.ERROR);
                        }
                    }
                    return;
            }
            throw new NotImplementedException($"SetProperty has not been setup to handle the property {name}.");
        }

        public abstract void PreInitialize(Vector<float> initializePosition);

        public abstract void Initialize(Vector<float> initializePosition);

    }
}
