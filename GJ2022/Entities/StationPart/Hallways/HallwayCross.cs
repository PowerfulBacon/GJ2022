﻿using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.StationPart.Hallways
{
    public class HallwayCross : StationPartEntity
    {

        public override Matrix ObjectMatrix { get; set; } = Matrix.GetScaleMatrix(3, 1, 1);

        public HallwayCross(Vector position) : base(position) { }

    }
}
