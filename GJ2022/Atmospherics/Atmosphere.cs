using GJ2022.Atmospherics.Gasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Atmospherics
{
    public class Atmosphere
    {

        public static Atmosphere SpaceAtmosphere = new Atmosphere();
        public static Atmosphere IdealAtmosphere = new Atmosphere(new PressurisedGas(Nitrogen.Singleton, ));

        //The atmospheric contents of this atmosphere
        public Dictionary<Gas, PressurisedGas> AtmosphericContents { get; } = new Dictionary<Gas, PressurisedGas>();

        //Temperature of this atmosphere
        public float KelvinTemperature { get; }

        public Atmosphere(params PressurisedGas[] gasses)
        {
            foreach (PressurisedGas gas in gasses)
            {
                AtmosphericContents.Add(gas.gas, gas);
            }
        }

    }
}
