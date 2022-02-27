using GJ2022.Entities.Items;
using GJ2022.Game.GameWorld;
using GJ2022.Managers.Stockpile;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Areas
{

    public class StockpileArea : Area
    {

        public override Renderable Renderable { get; set; } = new StandardRenderable("area_stockpile", true);

        private void StartManaging()
        {
            StockpileManager.AddStockpileArea(this);
        }

        public void RegisterItemsInStockpile()
        {
            
        }

        public void UnregisterItem(Item item)
        {
            StockpileManager.RemoveItem(item);
        }

        public override bool Destroy()
        {
            foreach (Item item in World.GetItems((int)Position[0], (int)Position[1]))
            {
                UnregisterItem(item);
            }
            return base.Destroy();
        }

        public override void Initialize(Vector<float> initializePosition)
        {
            base.Initialize(initializePosition);
            StartManaging();
        }
    }

}
