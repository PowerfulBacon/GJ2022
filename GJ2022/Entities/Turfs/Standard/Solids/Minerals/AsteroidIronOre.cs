using GJ2022.Audio;
using GJ2022.Entities.Items.Stacks;
using GJ2022.Entities.Items.Stacks.Ores;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Turfs.Standard.Solids.Minerals
{
    public class AsteroidIronOre : Asteroid
    {

        public AsteroidIronOre(int x, int y) : base(x, y)
        { }

        protected override string Texture => "mineral_iron";

        public override void Mine()
        {
            new IronOre(new Vector<float>(X, Y), 50, 4);
            base.Mine();
        }
    }
}
