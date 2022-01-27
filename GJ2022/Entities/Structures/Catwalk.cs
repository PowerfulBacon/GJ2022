using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Structures
{
    public class Catwalk : Structure
    {
        protected override Renderable Renderable { get; set; } = new StandardRenderable("catwalk");

        public Catwalk(Vector<float> position) : base(position, Layers.LAYER_CATWALK)
        { }

    }
}
