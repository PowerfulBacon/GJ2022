using GJ2022.Rendering.RenderSystems.Renderables;

namespace GJ2022.Entities.Turfs.Standard.Solids
{
    class Wall : Solid
    {

        protected override Renderable Renderable { get; set; } = new StandardRenderable($"wall");

        public Wall(int x, int y) : base(x, y)
        {
        }

    }
}
