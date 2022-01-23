using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Debug
{
    public class DebugSolid : DebugEntity
    {
        public DebugSolid(Vector position) : base(position)
        {}

        protected override string Texture { get; set; } = "stone";
    }
}
