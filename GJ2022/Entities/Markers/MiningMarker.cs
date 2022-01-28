﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        protected override Renderable Renderable { get; set; } = new StandardRenderable("mining_marker", true);

        public MiningMarker(Vector<float> position) : base(position, Layers.LAYER_MARKER)
        {
            //We were destroyed during init
            if (Destroyed)
                return;
            //Register signal
            Asteroid mineral = World.GetTurf((int)Position[0], (int)Position[1]) as Asteroid;
            if(mineral != null)
                SignalHandler.RegisterSignal(mineral, SignalHandler.Signal.SIGNAL_ENTITY_DESTROYED, (object source, object[] parameters) => {
                    if (Destroyed)
                        return SignalHandler.SignalResponse.NONE;
                    Destroy();
                    return SignalHandler.SignalResponse.NONE;
                });
        }

        public override bool IsValidPosition()
        {
            return World.GetTurf((int)Position[0], (int)Position[1]) is Asteroid;
        }

        public override bool Destroy()
        {
            Asteroid mineral = World.GetTurf((int)Position[0], (int)Position[1]) as Asteroid;
            if (mineral != null && SignalHandler.HasSignal(mineral, SignalHandler.Signal.SIGNAL_ENTITY_DESTROYED))
                SignalHandler.UnregisterSignal(mineral, SignalHandler.Signal.SIGNAL_ENTITY_DESTROYED);
            return base.Destroy();
        }

        public void HandleAction(Pawn pawn)
        {
            //Mine the rock at this position
            Asteroid mineral = World.GetTurf((int)Position[0], (int)Position[1]) as Asteroid;
            if (mineral == null || mineral.Destroyed)
            {
                Destroy();
                return;
            }
            //Destroy the mineral
            mineral.Mine();
        }

    }
}
