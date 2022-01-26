using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.WorldGeneration
{
    public abstract class WorldGenerator
    {

        public abstract void Generate(int min_x, int min_y, int max_x, int max_y);

    }
}
