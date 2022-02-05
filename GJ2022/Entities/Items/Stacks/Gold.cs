using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.Stacks
{
    public class Gold : Stack
    {

        public override Renderable Renderable { get; set; } = new StandardRenderable("gold");

        public override string UiTexture => "gold";

        public override string Name => "Gold";

        public Gold(Vector<float> position, int maxStackSize = 50, int stackSize = 1) : base(position, maxStackSize, stackSize)
        {
        }
    }
}
