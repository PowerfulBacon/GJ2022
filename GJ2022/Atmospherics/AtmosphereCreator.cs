using GJ2022.EntityLoading;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Atmospherics
{
    public class AtmosphereCreator : IInstantiatable
    {

        public float KelvinTemperature { private get; set; } = AtmosphericConstants.IDEAL_TEMPERATURE;
        public float LitreVolume { private get; set; }

        //Gasses
        public float Oxygen { private get; set; } = 0;
        public float CarbonDioxide { private get; set; } = 0;
        public float Hydrogen { private get; set; } = 0;

        public Atmosphere Generate()
        {
            Atmosphere atmosphere = new Atmosphere(KelvinTemperature);
            if (Oxygen > 0)
                atmosphere.SetMoles(Gasses.Oxygen.Singleton, Oxygen);
            if (CarbonDioxide > 0)
                atmosphere.SetMoles(Gasses.CarbonDioxide.Singleton, CarbonDioxide);
            if (Hydrogen > 0)
                atmosphere.SetMoles(Gasses.Hydrogen.Singleton, Hydrogen);
            atmosphere.SetVolume(LitreVolume);
            return atmosphere;
        }

        public void Initialize(Vector<float> initializePosition)
        {
            return;
        }

        public void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "KelvinTemperature":
                    KelvinTemperature = Convert.ToSingle(property);
                    return;
                case "LitreVolume":
                    LitreVolume = Convert.ToSingle(property);
                    return;
                case "Oxygen":
                    Oxygen = Convert.ToSingle(property);
                    return;
                case "CarbonDioxide":
                    CarbonDioxide = Convert.ToSingle(property);
                    return;
                case "Hydrogen":
                    Hydrogen = Convert.ToSingle(property);
                    return;
            }
            throw new NotImplementedException($"AtmosphereCreator doesn't have property {name}.");
        }
    }
}
