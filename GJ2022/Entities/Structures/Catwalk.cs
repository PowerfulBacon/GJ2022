using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Structures
{
    public class Catwalk : Structure
    {
        public override Renderable Renderable { get; set; } = new StandardRenderable("catwalk");

        public Catwalk(Vector<float> position) : base(position, Layers.LAYER_CATWALK)
        { }

    }
}
