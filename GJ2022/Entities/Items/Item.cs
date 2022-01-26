using GJ2022.Areas;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items
{
    public abstract class Item : Entity, IDestroyable, IMoveBehaviour
    {

        public bool Destroyed { get; private set; } = false;

        public Item(Vector<float> position) : base(position, Layers.LAYER_ITEM)
        { }

        public override bool Destroy()
        {
            base.Destroy();
            Destroyed = true;
            World.RemoveItem((int)Position[0], (int)Position[1], this);
            (World.GetArea((int)Position[0], (int)Position[1]) as StockpileArea)?.UnregisterItem(this);
            return true;
        }

        /// <summary>
        /// On move behaviour.
        /// Update ourself in the world list
        /// </summary>
        public void OnMoved(Vector<float> oldPosition)
        {
            if ((int)oldPosition[0] == (int)Position[0] && (int)oldPosition[1] == (int)Position[1])
                return;
            World.RemoveItem((int)oldPosition[0], (int)oldPosition[1], this);
            World.AddItem((int)Position[0], (int)Position[1], this);
            //Calculate stockpile
            (World.GetArea((int)oldPosition[0], (int)oldPosition[1]) as StockpileArea)?.UnregisterItem(this);
            (World.GetArea((int)Position[0], (int)Position[1]) as StockpileArea)?.RegisterItem(this);
        }

        public void OnMoved(Entity oldLocation)
        {
            if (oldLocation != null || oldLocation == Location)
                return;
            World.RemoveItem((int)Position[0], (int)Position[1], this);
            (World.GetArea((int)Position[0], (int)Position[1]) as StockpileArea)?.UnregisterItem(this);
        }
    }
}
