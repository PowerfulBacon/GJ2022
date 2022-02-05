﻿using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.Stacks
{
    public class Iron : Stack
    {

        public override Renderable Renderable { get; set; } = new StandardRenderable("iron");

        public override string UiTexture => "iron";

        public override string Name => "Iron";

        public Iron(Vector<float> position, int maxStackSize = 50, int stackSize = 1) : base(position, maxStackSize, stackSize)
        {
        }
    }
}
