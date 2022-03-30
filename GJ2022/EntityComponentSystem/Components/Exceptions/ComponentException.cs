using System;

namespace GJ2022.EntityComponentSystem.Components.Exceptions
{
    public class ComponentException : Exception
    {
        public ComponentException(string message) : base(message)
        { }
    }
}
