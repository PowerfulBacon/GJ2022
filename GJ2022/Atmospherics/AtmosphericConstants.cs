using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Atmospherics
{
    //https://github.com/BeeStation/BeeStation-Hornet/blob/master/code/__DEFINES/atmospherics.dm
    public static class AtmosphericConstants
    {

        //MOLES = PRESSURE * VOLUME / IDEAL * TEMP
        //PRESSURE * VOLUME = CONSTANT

        //Ideal pressure volume constant
        public const float PRESSURE_VOLUME_CONSTANT = IDEAL_PRESSURE * TILE_GAS_VOLUME;
        //Ideal pressure (kPa)
        public const float IDEAL_PRESSURE = 101.33f;
        //Ideal temperature of gasses (Kelvin)
        //20 degrees (room temperature)
        public const float IDEAL_TEMPERATURE = 295.15f;
        //How many liters of gas in a single tile (1 cubic meter)
        public const float TILE_GAS_VOLUME = 2500;

        private const float IDEAL_GAS_CONSTANT = (IDEAL_PRESSURE * TILE_GAS_VOLUME) / (41.27f * IDEAL_TEMPERATURE);

        public static float CalculateMoles(float pressure, float volume, float temperature)
        {
            //n = pv/Rt
            return (pressure * volume) / (IDEAL_GAS_CONSTANT * temperature);
        }

        public static float CalculatePressure(float volume)
        {
            return PRESSURE_VOLUME_CONSTANT / volume;
        }

        public static float CalculatePressure(float volume, float temperature, float moles)
        {
            //p = nRt/v
            return (moles * IDEAL_GAS_CONSTANT * temperature) / volume;
        }

        public static float CalculateTemperature(float pressure, float volume, float moles)
        {
            //t = pv/nR
            return (pressure * volume) / (IDEAL_GAS_CONSTANT * moles);
        }

        public static float CalculateVolume(float pressure, float temperature, float moles)
        {
            //v = nRt/p
            return (moles * IDEAL_GAS_CONSTANT * temperature) / pressure;
        }

        public static float CalculateVolume(float pressure)
        {
            return PRESSURE_VOLUME_CONSTANT / pressure;
        }

    }
}
