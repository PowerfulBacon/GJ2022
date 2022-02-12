using GJ2022.Rendering.RenderSystems.Renderables;

namespace GJ2022.Entities.Turfs.Standard.Floors
{
    public class GoldFloor : Floor
    {

        public override Renderable Renderable { get; set; } = new StandardRenderable($"gold_floor");

        public GoldFloor(int x, int y) : base(x, y)
        { }

    }
}
