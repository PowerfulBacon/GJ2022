using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Turfs.Standard.Floors
{
    public class AsteroidSand : Floor
    {

        private static Random random = new Random();

        protected override string Texture { get; } = $"asteroid_sand_{random.Next(1, 13)}";

        public AsteroidSand(int x, int y) : base(x, y)
        { }

    }
}
