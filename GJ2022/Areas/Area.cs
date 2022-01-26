using GJ2022.Entities;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Areas
{
    public abstract class Area : Entity
    {

        protected Area(Vector<float> position) : base(position, Layers.LAYER_AREA)
        {
            World.SetArea((int)position[0], (int)position[1], this);
        }

    }
}
