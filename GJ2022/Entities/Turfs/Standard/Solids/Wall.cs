using GJ2022.Rendering.RenderSystems.Renderables;

namespace GJ2022.Entities.Turfs.Standard.Solids
{
    public class Wall : Solid
    {

        public override Renderable Renderable { get; set; } = new StandardRenderable($"wall");

        public Wall(int x, int y) : base(x, y)
        {
        }

    }
}
