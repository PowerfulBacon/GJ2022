using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Atmospherics.Gasses
{
    public class Oxygen : Gas
    {

        public static Oxygen Singleton { get; } = new Oxygen();

    }
}
