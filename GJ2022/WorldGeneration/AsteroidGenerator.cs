using GJ2022.EntityLoading;
using LibNoise;

namespace GJ2022.WorldGeneration
{
    public class AsteroidGenerator : WorldGenerator
    {

        private Perlin oreGenerator;
        private Perlin perlinGenerator;

        public AsteroidGenerator()
        {
            oreGenerator = new Perlin();
            oreGenerator.Frequency = 1.5;
            oreGenerator.Persistence = 1;
            oreGenerator.Lacunarity = 0.2;
            oreGenerator.OctaveCount = 8;
            oreGenerator.Seed = 4;
            oreGenerator.NoiseQuality = NoiseQuality.Standard;

            perlinGenerator = new Perlin();
            perlinGenerator.Frequency = 1;
            perlinGenerator.Persistence = 1;
            perlinGenerator.Lacunarity = 0.1;
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
            double value = perlinGenerator.GetValue(x, y, 0);
            double oreValue = oreGenerator.GetValue(x, y, 0);
            if (System.Math.Abs(x) < 20 && System.Math.Abs(y) < 20)
                return;
            if (value > 0.8)
                if (oreValue > 0.8)
                    EntityCreator.CreateEntity("AsteroidIronOre", new Utility.MathConstructs.Vector<float>(x, y));
                else
                    EntityCreator.CreateEntity("Asteroid", new Utility.MathConstructs.Vector<float>(x, y));
            else if (value > 0.65)
                EntityCreator.CreateEntity("AsteroidSand", new Utility.MathConstructs.Vector<float>(x, y));
        }

    }
}
