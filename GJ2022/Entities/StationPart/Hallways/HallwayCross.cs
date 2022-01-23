using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.StationPart.Hallways
{
    public class HallwayCross : StationPartEntity
    {

        public override Matrix ObjectMatrix { get; set; } = Matrix.GetScaleMatrix(3, 1, 1);

        public HallwayCross(Vector position) : base(position) { }

    }
}
