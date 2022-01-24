using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.Stacks
{
    public class Iron : Stack
    {
        public Iron(Vector<float> position, int maxStackSize = 50, int stackSize = 1) : base(position, maxStackSize, stackSize)
        {
        }

        public override string Texture => "iron";

    }
}
