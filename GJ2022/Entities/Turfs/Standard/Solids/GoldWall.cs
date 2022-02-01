using GJ2022.Rendering.RenderSystems.Renderables;

namespace GJ2022.Entities.Turfs.Standard.Solids
{
    public class GoldWall : Solid
    {

        protected override Renderable Renderable { get; set; } = new StandardRenderable($"gold_wall");

        public GoldWall(int x, int y) : base(x, y)
        {
        }

    }
}
