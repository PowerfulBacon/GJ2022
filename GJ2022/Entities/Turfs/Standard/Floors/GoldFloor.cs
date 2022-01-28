using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Turfs.Standard.Floors
{
    class GoldFloor : Floor
    {

        protected override string Texture { get; } = "gold_floor";

        public GoldFloor(int x, int y) : base(x, y)
        { }

    }
}
