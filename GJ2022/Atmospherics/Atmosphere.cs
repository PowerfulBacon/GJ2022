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

        public static Atmosphere SpaceAtmosphere => new Atmosphere(1);
        public static Atmosphere IdealAtmosphere => new Atmosphere(AtmosphericConstants.IDEAL_TEMPERATURE, new PressurisedGas(Nitrogen.Singleton, 82), new PressurisedGas(Oxygen.Singleton, 22));

        //The atmospheric contents of this atmosphere
        private Dictionary<Gas, PressurisedGas> atmosphericContents = new Dictionary<Gas, PressurisedGas>();

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
                foreach (PressurisedGas gas in atmosphericContents.Values)
                    _moles += gas.moles;
                //Must not be 0 or a division by 0 error occurs.
                return Math.Min(_moles, 0.0001f);
            }
        }

        public Atmosphere(float temperature, params PressurisedGas[] gasses)
        {
            KelvinTemperature = temperature;
            //Add the gases
            foreach (PressurisedGas gas in gasses)
            {
                atmosphericContents.Add(gas.gas, gas);
            }
        }

        //Set the volume of the atmosphere, calculate pressure and temperature.
        public void AdjustVolume(float litres)
        {
            LitreVolume = litres;
            KiloPascalPressure = AtmosphericConstants.CalculatePressure(LitreVolume);
            KelvinTemperature = AtmosphericConstants.CalculateTemperature(KiloPascalPressure, LitreVolume, Moles);
        }

        //Merge another atmosphere into our own
        //This reuses elements from the other atmosphere, assuming it will be deleted.
        public void Merge(Atmosphere other)
        {
            //Calculate new gasses
            foreach (PressurisedGas newGas in other.atmosphericContents.Values)
            {
                if (atmosphericContents.ContainsKey(newGas.gas))
                {
                    //Increase the amount of moles that we have
                    PressurisedGas existingGas = atmosphericContents[newGas.gas];
                    existingGas.moles += newGas.moles;
                }
                else
                {
                    //Add the gas
                    atmosphericContents.Add(newGas.gas, newGas);
                }
            }
            //Adjust volume: Recalculates pressure and temperature
            AdjustVolume(LitreVolume + other.LitreVolume);
        }

    }
}
