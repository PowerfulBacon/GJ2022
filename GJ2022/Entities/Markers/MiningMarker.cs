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
            World.Current.GetTurf((int)Position.X, (int)Position.Y).RegisterSignal(Signal.SIGNAL_ENTITY_DESTROYED, 0, DestroyMarker);
        }

        public override bool IsValidPosition()
        {
            return World.Current.GetThings("Mine", (int)Position.X, (int)Position.Y).Count > 0;
        }

        public override bool Destroy()
        {
            World.Current.GetTurf((int)Position.X, (int)Position.Y)?.UnregisterSignal(Signal.SIGNAL_ENTITY_DESTROYED, DestroyMarker);
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
            new AudioSource().PlaySound($"effects/picaxe{World.Random.Next(1, 4)}.wav", Position.X, Position.Y);
            //Perform mining
            World.Current.GetTurf((int)Position.X, (int)Position.Y)?.SendSignal(Signal.SIGNAL_ENTITY_MINE, pawn);
            if(!Destroyed)
                Destroy();
        }

    }
}
