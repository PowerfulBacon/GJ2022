using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Managers;

namespace GJ2022.Entities.Turfs
{

    public abstract class Turf : IDestroyable
    {

        //Position of the turf
        public int X { get; }
        public int Y { get; }

        public bool Destroyed { get; set; } = false;

        public Turf(int x, int y)
        {
            X = x;
            Y = y;
            //Destroy the old turf
            World.GetTurf(x, y)?.Destroy();
            //Set the new turf
            World.SetTurf(x, y, this);
        }

        //Set destroyed
        public virtual bool Destroy()
        {
            //Unregister signals
            SignalHandler.SendSignal(this, SignalHandler.Signal.SIGNAL_ENTITY_DESTROYED);
            //Unregister all signals
            SignalHandler.UnregisterAll(this);
            //Dereference
            World.SetTurf(X, Y, null);
            //Set destroyed
            Destroyed = true;
            return true;
        }

    }

}
