using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.Stacks
{
    public class Iron : Stack
    {

        protected override Renderable Renderable { get; set; } = new StandardRenderable("iron");

        public Iron(Vector<float> position, int maxStackSize = 50, int stackSize = 1) : base(position, maxStackSize, stackSize)
        {
        }
    }
}
