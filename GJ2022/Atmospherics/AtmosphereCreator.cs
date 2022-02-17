using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Atmospherics
{
    public class AtmosphereCreator
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

    }
}
