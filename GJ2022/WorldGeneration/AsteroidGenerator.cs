using GJ2022.Entities.Turfs.Standard.Solids;
using LibNoise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.WorldGeneration
{
    public class AsteroidGenerator : WorldGenerator
    {

        private Perlin perlinGenerator;

        public AsteroidGenerator()
        {
            perlinGenerator = new Perlin();
            perlinGenerator.Frequency = 1;
            perlinGenerator.Persistence = 1;
            perlinGenerator.Lacunarity = 0.2;
            perlinGenerator.OctaveCount = 8;
            perlinGenerator.Seed = 3;
            perlinGenerator.NoiseQuality = NoiseQuality.Standard;
        }

        public override void Generate(int min_x, int min_y, int max_x, int max_y)
        {
            for (int x = min_x; x <= max_x; x++)
            {
                for (int y = min_y; y <= max_y; y++)
                {
                    GeneratePosition(x, y);
                }
            }
        }

        public void GeneratePosition(int x, int y)
        {
            if (perlinGenerator.GetValue(x, y, 0) < 0.8f)
                return;
            if (System.Math.Abs(x) < 20 && System.Math.Abs(y) < 20)
                return;
            new Asteroid(x, y);
        }

    }
}
