using GJ2022.Atmospherics.Gasses;
using GJ2022.Entities.Structures;
using GJ2022.Entities.Turfs;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Debug
{
    class DebugCanister : Structure
    {
        public DebugCanister(Vector<float> position) : base(position, Layers.LAYER_STRUCTURE)
        {
            Turf turf = World.GetTurf((int)position[0], (int)position[1]);
            if (turf == null)
                return;
            if (turf.Atmosphere == null)
                return;
            turf.Atmosphere.ContainedAtmosphere.SetMoles(Oxygen.Singleton, turf.Atmosphere.ContainedAtmosphere.GetMoles(Oxygen.Singleton) + 100);
            turf.Atmosphere.ContainedAtmosphere.SetTemperature(Atmospherics.AtmosphericConstants.IDEAL_TEMPERATURE);
            Log.WriteLine($"Atmos ({turf.Atmosphere.BlockId}): Oxygen moles {turf.Atmosphere.ContainedAtmosphere.GetMoles(Oxygen.Singleton)}/{turf.Atmosphere.ContainedAtmosphere.Moles}, Temperature {turf.Atmosphere.ContainedAtmosphere.KelvinTemperature}, Pressure: {turf.Atmosphere.ContainedAtmosphere.KiloPascalPressure}, Volume {turf.Atmosphere.ContainedAtmosphere.LitreVolume}");
        }

        protected override Renderable Renderable { get; set; } = new StandardRenderable("canister");

    }
}
