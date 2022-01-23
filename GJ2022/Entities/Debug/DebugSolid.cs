using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Debug
{
    public class DebugSolid : DebugEntity
    {
        public DebugSolid(Vector position) : base(position)
        { }

        protected override string Texture { get; set; } = "stone";
    }
}
