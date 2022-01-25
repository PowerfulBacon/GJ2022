using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Utility.Helpers
{
    public static class CoordinateHelper
    {

        public static Vector<float> PixelsToScreen(float inputA, float inputB)
        {
            return new Vector<float>(inputA / 1080.0f, inputB / 1080.0f);
        }

        public static Vector<float> PixelsToScreen(Vector<float> input)
        {
            return input / 1080.0f;
        }

        public static float PixelsToScreen(float input)
        {
            return input / 1080.0f;
        }

    }
}
