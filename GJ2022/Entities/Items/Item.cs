using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items
{
    public abstract class Item : Entity, IDestroyable
    {

        public bool Destroyed { get; private set; } = false;

        public Item(Vector<float> position) : base(position, Layers.LAYER_ITEM)
        { }

        public override bool Destroy()
        {
            base.Destroy();
            Destroyed = true;
            return true;
        }

    }
}
