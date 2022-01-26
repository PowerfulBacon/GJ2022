using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Atmospherics
{
    public static class AtmosphericConstants
    {

        //Ideal pressure
        public const float IDEAL_PRESSURE = 101.33f;
        //Ideal temperature of gasses
        public const float IDEAL_TEMPERATURE = 295.15f;
        //How many liters of gas in a single tile (1 cubic meter)
        public const float TILE_GAS_VOLUME = 1000;

        private const float IDEAL_GAS_CONSTANT = (IDEAL_PRESSURE * TILE_GAS_VOLUME) / (41.27f * IDEAL_TEMPERATURE)

        public static float CalculateMoles(float pressure, float volume, float temperature)
        {
            return (pressure * volume) / (IDEAL_GAS_CONSTANT * temperature);
        }

    }
}
