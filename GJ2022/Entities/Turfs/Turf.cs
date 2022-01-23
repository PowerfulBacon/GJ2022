using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Turfs
{

    public abstract class Turf : IDestroyable
    {

        //Position of the turf
        public int X { get; }
        public int Y { get; }

        //Is this destroyed?
        private bool destroyed = false;

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
            destroyed = true;
            return true;
        }

        //Are we destroyed?
        public bool IsDestroyed()
        {
            return destroyed;
        }
    }

}
