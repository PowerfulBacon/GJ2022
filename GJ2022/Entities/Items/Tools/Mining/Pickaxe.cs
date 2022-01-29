﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.Tools.Mining
{
    public class Pickaxe : Tool
    {

        public Pickaxe(Vector<float> position) : base(position)
        {
        }

        public override ToolBehaviours ToolBehaviour => ToolBehaviours.TOOL_PICKAXE;

        public override string Name => "Pickaxe";

        public override string UiTexture => "pickaxe";

        protected override Renderable Renderable { get; set; } = new StandardRenderable("pickaxe");
    }
}
