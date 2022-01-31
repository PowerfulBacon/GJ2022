using GJ2022.Entities.Blueprints;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;
using System.Linq;

namespace GJ2022.Entities.Markers
{
    public class CancelMarker : Marker
    {

        public CancelMarker(Vector<float> position) : base(position, Layers.LAYER_MARKER)
        {
            //Destroy blueprints
            if (PawnControllerSystem.QueuedBlueprints.ContainsKey(position))
            {
                foreach (Blueprint blueprint in PawnControllerSystem.QueuedBlueprints[position].Values.ToList())
                {
                    blueprint.Destroy();
                }
            }
            Destroy();
        }

        protected override Renderable Renderable { get; set; } = new StandardRenderable("cancel");

        public override bool IsValidPosition()
        {
            return true;
        }
    }
}
