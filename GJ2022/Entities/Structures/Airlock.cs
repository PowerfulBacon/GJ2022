using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Structures
{
    public class Airlock : Structure, ISolid
    {

        protected override Renderable Renderable { get; set; } = new StandardRenderable("door_closed");

        public Airlock(Vector<float> position) : base(position, Layers.LAYER_STRUCTURE)
        {
        }

    }
}
