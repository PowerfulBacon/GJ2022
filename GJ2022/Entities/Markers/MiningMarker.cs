using GJ2022.Audio;
using GJ2022.Components;
using GJ2022.Entities.Pawns;
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
            World.GetTurf((int)Position[0], (int)Position[1]).RegisterSignal(Signal.SIGNAL_ENTITY_DESTROYED, 0, DestroyMarker);
        }

        public override bool IsValidPosition()
        {
            return World.GetThings("Mine", (int)Position[0], (int)Position[1]).Count > 0;
        }

        public override bool Destroy()
        {
            World.GetTurf((int)Position[0], (int)Position[1])?.UnregisterSignal(Signal.SIGNAL_ENTITY_DESTROYED, DestroyMarker);
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
            new AudioSource().PlaySound($"effects/picaxe{World.Random.Next(1, 4)}.wav", Position[0], Position[1]);
            //Perform mining
            World.GetTurf((int)Position[0], (int)Position[1])?.SendSignal(Signal.SIGNAL_ENTITY_MINE, pawn);
            if(!Destroyed)
                Destroy();
        }

    }
}
