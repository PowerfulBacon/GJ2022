using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.StockParts.Cells
{
    class StandardCell : Cell
    {

        public StandardCell(Vector<float> position) : base(position)
        { }

        public StandardCell(Entity location) : base(location)
        { }

        public override float MaxCharge => 1000;

        public override Renderable Renderable { get; set; } = new StandardRenderable("power.cell");

    }
}
