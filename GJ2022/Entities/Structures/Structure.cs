using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Structures
{
    public abstract class Structure : Entity, IDestroyable
    {

        public Structure(Vector<float> position, float layer) : base(position, layer)
        {
            //Add the structure to the world list
            World.AddStructure((int)position[0], (int)position[1], this);
        }

        public bool Destroyed { get; private set; } = false;

        public override bool Destroy()
        {
            Destroyed = true;
            World.RemoveStructure((int)Position[0], (int)Position[1], this);
            return base.Destroy();
        }
    }
}
