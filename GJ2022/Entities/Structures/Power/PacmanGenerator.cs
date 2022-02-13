using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Structures.Power
{
    class PacmanGenerator : Structure
    {

        public PacmanGenerator(Vector<float> position, float layer) : base(position, layer)
        { }

        public override Renderable Renderable { get; set; } = new StandardRenderable("power.portgen0_0");

    }
}
