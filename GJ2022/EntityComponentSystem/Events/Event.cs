using GJ2022.EntityComponentSystem.Entities;

namespace GJ2022.EntityComponentSystem.Events
{
    public abstract class Event
    {

        /// <summary>
        /// Raise this event against a specified target
        /// </summary>
        public void Raise(Entity target)
        {
            target.HandleSignal(this);
        }

    }
}
