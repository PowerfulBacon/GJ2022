﻿using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Markers
{
    public class CancelMarker : Marker
    {

        public CancelMarker(Vector<float> position) : base(position, Layers.LAYER_MARKER)
        {
            Destroy();
        }

        protected override Renderable Renderable { get; set; } = new StandardRenderable("cancel");

        public override bool IsValidPosition()
        {
            return true;
        }
    }
}
