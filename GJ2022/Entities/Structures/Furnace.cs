using GJ2022.Entities.Items.Stacks.Ores;
using GJ2022.Entities.Pawns;
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
    public class Furnace : Structure
    {

        public Furnace(Vector<float> position) : base(position, Layers.LAYER_STRUCTURE)
        {
        }

        protected override Renderable Renderable { get; set; } = new StandardRenderable("furnace");

        public void Smelt(Pawn user, IronOre ore)
        {
            ore.DoSmelt(user);
        }

    }
}
