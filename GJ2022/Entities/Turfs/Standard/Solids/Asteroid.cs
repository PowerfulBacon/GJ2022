using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Turfs.Standard.Solids
{
    public class Asteroid : Solid
    {

        protected override string Texture { get; } = "stone";

        public Asteroid(int x, int y) : base(x, y)
        {
        }

    }
}
