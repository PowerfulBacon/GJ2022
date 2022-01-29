using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.Tools
{
    public abstract class Tool : Item
    {

        public abstract ToolBehaviours ToolBehaviour { get; }

        public Tool(Vector<float> position) : base(position)
        {
        }

    }
}
