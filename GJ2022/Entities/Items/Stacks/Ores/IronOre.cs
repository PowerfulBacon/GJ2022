using GJ2022.Entities.Pawns;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.Stacks.Ores
{
    public class IronOre : Stack
    {
        public IronOre(Vector<float> position, int maxStackSize = 50, int stackSize = 1) : base(position, maxStackSize, stackSize)
        { }

        public override string Name => "Iron Ore";

        public override string UiTexture => "iron_ore";

        public override Renderable Renderable { get; set; } = new StandardRenderable("iron_ore");

        public void DoSmelt(Pawn user)
        {
            new Iron(user.Position, 50, StackSize);
            Destroy();
        }

    }
}
