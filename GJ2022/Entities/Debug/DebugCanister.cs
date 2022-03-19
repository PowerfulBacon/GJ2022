using GJ2022.Atmospherics.Gasses;
using GJ2022.Entities.Structures;
using GJ2022.Entities.Turfs;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Debug
{
    public class DebugCanister : Structure
    {

        protected virtual Gas SpawnedGas { get; } = Oxygen.Singleton;

        public DebugCanister(Vector<float> position) : base(position, Layers.LAYER_STRUCTURE)
        {
            Turf turf = World.Current.GetTurf((int)position.X, (int)position.Y);
            if (turf == null)
                return;
            if (turf.Atmosphere == null)
                return;
            turf.Atmosphere.ContainedAtmosphere.SetMoles(SpawnedGas, turf.Atmosphere.ContainedAtmosphere.GetMoles(SpawnedGas) + 100);
            turf.Atmosphere.ContainedAtmosphere.SetTemperature(Atmospherics.AtmosphericConstants.IDEAL_TEMPERATURE);
            Log.WriteLine($"Atmos ({turf.Atmosphere.BlockId}): Oxygen moles {turf.Atmosphere.ContainedAtmosphere.GetMoles(SpawnedGas)}/{turf.Atmosphere.ContainedAtmosphere.Moles}, Temperature {turf.Atmosphere.ContainedAtmosphere.KelvinTemperature}, Pressure: {turf.Atmosphere.ContainedAtmosphere.KiloPascalPressure}, Volume {turf.Atmosphere.ContainedAtmosphere.LitreVolume}");
        }

        public override Renderable Renderable { get; set; } = new StandardRenderable("canister");

    }

    public class DebugCanisterHydrogen : DebugCanister
    {
        protected override Gas SpawnedGas => Hydrogen.Singleton;

        public DebugCanisterHydrogen(Vector<float> position) : base(position) { }

    }

}