using GJ2022.Audio;
using GJ2022.Entities.Turfs.Standard.Floors;
using GJ2022.Rendering.RenderSystems.Renderables;
using System;

namespace GJ2022.Entities.Turfs.Standard.Solids
{
    public class Asteroid : Solid
    {

        private static Random random = new Random();

        protected override Renderable Renderable { get; set; } = new StandardRenderable($"stone");

        public Asteroid(int x, int y) : base(x, y)
        {
        }

        public override bool Destroy()
        {
            return base.Destroy();
        }

        public virtual void Mine()
        {
            new AudioSource().PlaySound($"effects/picaxe{random.Next(1, 4)}.wav", X, Y);
            new AsteroidSand(X, Y);
        }

    }
}
