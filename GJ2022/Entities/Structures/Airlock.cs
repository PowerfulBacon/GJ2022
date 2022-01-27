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
    public class Airlock : Structure
    {

        protected override Renderable Renderable { get; set; } = new StandardRenderable("door_closed");

        public Airlock(Vector<float> position) : base(position, Layers.LAYER_STRUCTURE)
        {
        }

    }
}
