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

        public static Atmosphere SpaceAtmosphere = new Atmosphere(0);
        public static Atmosphere IdealAtmosphere = new Atmosphere(AtmosphericConstants.IDEAL_TEMPERATURE, new PressurisedGas(Nitrogen.Singleton, 82), new PressurisedGas(Oxygen.Singleton, 22));

        //The atmospheric contents of this atmosphere
        public Dictionary<Gas, PressurisedGas> AtmosphericContents { get; } = new Dictionary<Gas, PressurisedGas>();

        //Temperature of this atmosphere
        public float KelvinTemperature { get; private set; }

        //The volume in litres of this atmosphere
        public float LitreVolume { get; private set; }

        //Pressure in kPa
        public float KiloPascalPressure { get; private set; }

        //Moles of gas in this atmosphere
        public float Moles
        {
            get
            {
                float _moles = 0;
                foreach (PressurisedGas gas in AtmosphericContents.Values)
                    _moles += gas.moles;
                return _moles;
            }
        }

        public Atmosphere(float temperature, params PressurisedGas[] gasses)
        {
            KelvinTemperature = temperature;
            //Add the gases
            foreach (PressurisedGas gas in gasses)
            {
                AtmosphericContents.Add(gas.gas, gas);
            }
        }

        //Set the volume of the atmosphere, calculate pressure and temperature.
        public void AdjustVolume(float litres)
        {
            LitreVolume = litres;
            KiloPascalPressure = AtmosphericConstants.CalculatePressure(LitreVolume);
            KelvinTemperature = AtmosphericConstants.CalculateTemperature(KiloPascalPressure, LitreVolume, Moles);
        }

    }
}
