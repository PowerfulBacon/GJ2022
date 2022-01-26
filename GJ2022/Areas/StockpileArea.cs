using GJ2022.Entities;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Areas
{

    public class StockpileArea : Area
    {

        protected override Renderable Renderable { get; set; } = new StandardRenderable("area_stockpile", true);

        public StockpileArea(Vector<float> position) : base(position)
        { }

        public StockpileArea(Entity location) : base(location)
        { }

    }

}
