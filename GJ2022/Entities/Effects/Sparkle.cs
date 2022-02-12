using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Effects
{
    public class Sparkle : Effect
    {
        public Sparkle(Vector<float> position) : base(position, Layers.LAYER_EFFECT)
        {
        }

        protected override int ExistanceTime => 1000;

        public override Renderable Renderable { get; set; } = new StandardRenderable("sparkle");

    }
}
