using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Entities.Turfs.Standard.Solids;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Markers
{
    public class MiningMarker : Marker
    {

        public MiningMarker(Vector<float> position) : base(position, Layers.LAYER_MARKER)
        { }

        protected override Renderable Renderable { get; set; } = new StandardRenderable("mining_marker");

        public override bool IsValidPosition()
        {
            return World.GetTurf((int)Position[0], (int)Position[1])?.GetType() == typeof(Asteroid);
        }
    }
}
