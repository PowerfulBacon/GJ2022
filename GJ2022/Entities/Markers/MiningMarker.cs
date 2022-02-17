using GJ2022.Components;
using GJ2022.Entities.Pawns;
using GJ2022.Entities.Turfs.Standard.Solids;
using GJ2022.Game.GameWorld;
using GJ2022.Managers;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Markers
{
    public class MiningMarker : Marker
    {

        public override Renderable Renderable { get; set; } = new StandardRenderable("mining_marker", true);

        public MiningMarker(Vector<float> position) : base(position, Layers.LAYER_MARKER)
        {
            //We were destroyed during init
            if (Destroyed)
                return;
            //Register signal
            if (World.GetTurf((int)Position[0], (int)Position[1]) is Asteroid mineral)
                mineral.RegisterSignal(Signal.SIGNAL_ENTITY_DESTROYED, 0, DestroyMarker);
        }

        public override bool IsValidPosition()
        {
            return World.GetTurf((int)Position[0], (int)Position[1]) is Asteroid;
        }

        public override bool Destroy()
        {
            if (World.GetTurf((int)Position[0], (int)Position[1]) is Asteroid mineral)
                mineral.UnregisterSignal(Signal.SIGNAL_ENTITY_DESTROYED, DestroyMarker);
            return base.Destroy();
        }

        private object DestroyMarker(object source, object[] parameters)
        {
            if (Destroyed)
                return null;
            Destroy();
            return null;
        }

        public void HandleAction(Pawn pawn)
        {
            //Mine the rock at this position
            if (!(World.GetTurf((int)Position[0], (int)Position[1]) is Asteroid mineral) || mineral.Destroyed)
            {
                Destroy();
                return;
            }
            //Destroy the mineral
            mineral.Mine();
        }

    }
}
