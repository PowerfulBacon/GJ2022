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
