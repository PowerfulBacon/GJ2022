using GJ2022.Entities;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Areas
{
    public abstract class Area : Entity
    {

        protected Area(Vector<float> position) : base(position, Layers.LAYER_AREA)
        { }

        protected Area(Entity location) : base(location, Layers.LAYER_AREA)
        { }

    }
}
