using GJ2022.Entities;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Areas
{
    public abstract class Area : Entity, IDestroyable
    {

        protected Area(Vector<float> position) : base(position, Layers.LAYER_AREA)
        {
            World.GetArea((int)position[0], (int)position[1])?.Destroy();
            World.SetArea((int)position[0], (int)position[1], this);
        }

        public bool Destroyed { get; private set; } = false;

        public override bool Destroy()
        {
            Destroyed = true;
            World.SetArea((int)Position[0], (int)Position[1], null);
            return base.Destroy();
        }
    }
}
