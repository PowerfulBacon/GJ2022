using GJ2022.Rendering.RenderSystems.Renderables;

namespace GJ2022.Entities.Turfs.Standard.Floors
{
    class GoldFloor : Floor
    {

        protected override Renderable Renderable { get; set; } = new StandardRenderable($"gold_floor");

        public GoldFloor(int x, int y) : base(x, y)
        { }

    }
}
