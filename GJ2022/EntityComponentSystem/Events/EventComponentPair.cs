using System;

namespace GJ2022.EntityComponentSystem.Events
{
    internal struct EventComponentPair
    {

        public Type EventType { get; }

        public Type ComponentType { get; }

        public EventComponentPair(Type eventType, Type componentType)
        {
            EventType = eventType;
            ComponentType = componentType;
        }
    }
}
