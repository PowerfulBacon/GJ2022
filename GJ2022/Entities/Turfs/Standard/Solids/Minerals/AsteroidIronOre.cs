using GJ2022.Entities.Items.Stacks.Ores;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Turfs.Standard.Solids.Minerals
{
    public class AsteroidIronOre : Asteroid
    {

        public override Renderable Renderable { get; set; } = new StandardRenderable($"mineral_iron");

        public AsteroidIronOre(int x, int y) : base(x, y)
        { }

        public override void Mine()
        {
            new IronOre(new Vector<float>(X, Y), 50, 4);
            base.Mine();
        }
    }
}
