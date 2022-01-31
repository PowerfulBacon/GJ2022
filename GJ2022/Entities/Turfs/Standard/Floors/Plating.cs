using GJ2022.Rendering.RenderSystems.Renderables;

namespace GJ2022.Entities.Turfs.Standard.Floors
{
    public class Plating : Floor
    {

        protected override Renderable Renderable { get; set; } = new StandardRenderable($"plating");

        public Plating(int x, int y) : base(x, y)
        { }

    }
}
