using GJ2022.Rendering.RenderSystems.Renderables;
using System;

namespace GJ2022.Entities.Turfs.Standard.Floors
{
    public class AsteroidSand : Turf
    {

        private static Random random = new Random();

        public override Renderable Renderable { get; set; } = new StandardRenderable($"asteroid_sand_{random.Next(1, 13)}");

        public AsteroidSand(int x, int y) : base(x, y)
        { }

    }
}
