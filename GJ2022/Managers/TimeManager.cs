using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Managers
{
    public static class TimeManager
    {

        public static double TimeMultiplier { get; set; } = 1;

        public static double Time { get; private set; } = 0;

        public static void UpdateTime(double deltaRealTime)
        {
            Time += deltaRealTime * TimeMultiplier;
        }

    }
}
