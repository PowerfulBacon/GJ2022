﻿using GJ2022.Atmospherics.Gasses;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Pawns;
using GJ2022.Entities.Pawns.Health.Injuries.Instances.Generic;
using GJ2022.Entities.Turfs;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Subsystems.Processing;
using GJ2022.Utility.MathConstructs;
using System;

namespace GJ2022.Entities.Structures
{
    public class Fire : Structure, IProcessable
    {

        private const float OXYGEN_BURN_RATE = 0.2f;
        private const float FIRE_SPREAD_CHANCE = 0.01f;  //Chance per mole of oxygen to spread (1 mole = 1%, 100 moles = 100%)

        private static Random Random = new Random();

        private static PositionBasedBinaryList<Fire> GlobalFires = new PositionBasedBinaryList<Fire>();

        public Fire() : base()
        { }

        public override void Initialize(Vector<float> initializePosition)
        {
            FireProcessingSystem.Singleton.StartProcessing(this);
            GlobalFires.Add((int)Position.X, (int)Position.Y, this);
            base.Initialize(initializePosition);
        }

        public Fire(Vector<float> position) : base(position, Layers.LAYER_FIRE)
        {
            FireProcessingSystem.Singleton.StartProcessing(this);
            GlobalFires.Add((int)position.X, (int)position.Y, this);
        }

        public override Renderable Renderable { get; set; } = new StandardRenderable("fire", true);

        public override bool Destroy()
        {
            FireProcessingSystem.Singleton.StopProcessing(this);
            GlobalFires.Remove((int)Position.X, (int)Position.Y);
            return base.Destroy();
        }

        public void Process(float deltaTime)
        {
            //Get the current turf
            Turf turf = World.Current.GetTurf((int)Position.X, (int)Position.Y);
            //Check turf atmos
            if (turf == null || turf.Atmosphere == null)
            {
                Destroy();
                return;
            }
            //Check if we can keep burning
            //Less moles = more chance to randomly stop burning
            float molesOfOxygenLeft = turf.Atmosphere.ContainedAtmosphere.GetMoles(Oxygen.Singleton);
            float molesOfFlammableGasLeft = turf.Atmosphere.ContainedAtmosphere.GetMoles(Hydrogen.Singleton);
            if ((molesOfOxygenLeft < 20 && 0.05f * molesOfOxygenLeft < Random.NextDouble())
                || (molesOfFlammableGasLeft < 20 && 0.05f * molesOfFlammableGasLeft < Random.NextDouble()))
            {
                Destroy();
                return;
            }
            //Hurt pawns
            foreach (Pawn pawn in World.Current.GetPawns(turf.X, turf.Y))
            {
                pawn.PawnBody.ApplyDamageRandomly(new Burn(0.5f * deltaTime));
            }
            //Consume oxygen
            turf.Atmosphere.ContainedAtmosphere.SetMoles(Oxygen.Singleton, Math.Max(molesOfOxygenLeft - (OXYGEN_BURN_RATE * deltaTime), 0.0f));
            turf.Atmosphere.ContainedAtmosphere.SetMoles(Hydrogen.Singleton, Math.Max(molesOfFlammableGasLeft - (OXYGEN_BURN_RATE * deltaTime), 0.0f));
            //Increase temperature
            turf.Atmosphere.ContainedAtmosphere.SetTemperature(turf.Atmosphere.ContainedAtmosphere.KelvinTemperature + (25000 * deltaTime / turf.Atmosphere.ContainedAtmosphere.LitreVolume));
            //Spread to surrounding tiles
            Vector<int>[] directions = new Vector<int>[] {
                new Vector<int>(1, 0),
                new Vector<int>(0, 1),
                new Vector<int>(-1, 0),
                new Vector<int>(0, -1)
            };
            //Get position
            int x = (int)Position.X;
            int y = (int)Position.Y;
            //Check if we want to spread
            if (Random.NextDouble() > FIRE_SPREAD_CHANCE * molesOfOxygenLeft)
                return;
            //Choose a random direction to spread
            int i = Random.Next(0, 4);
            //Go through directions
            Vector<int> spreadDirction = directions[i];
            //Check for atmospheric flow allowance
            if (!World.Current.AllowsAtmosphericFlow(x + spreadDirction[0], y + spreadDirction[1]))
                return;
            //Check for fires
            if (GlobalFires.Get(x + spreadDirction[0], y + spreadDirction[1]) != null)
                return;
            //Spread
            new Fire(new Vector<float>(x + spreadDirction[0], y + spreadDirction[1]));
        }

    }
}
