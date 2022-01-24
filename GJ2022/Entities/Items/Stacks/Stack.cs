using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.Stacks
{
    public abstract class Stack : Item
    {

        protected Stack(Vector<float> position, int maxStackSize = 50, int stackSize = 1) : base(position)
        {
            MaxStackSize = maxStackSize;
            StackSize = stackSize;
        }

        public int MaxStackSize { get; }

        public int StackSize { get; set; }

    }
}
