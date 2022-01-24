using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Debug
{
    public class DebugSolid : DebugEntity, ISolid
    {
        public DebugSolid(Vector<float> position) : base(position)
        { }

        protected override string Texture { get; set; } = "stone";
    }
}
