using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;

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
            //Dereference
            World.SetTurf(X, Y, null);
            //Set destroyed
            Destroyed = true;
            return true;
        }

    }

}
