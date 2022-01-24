using System;
using System.Collections.Generic;

namespace GJ2022.Game.Construction.Cost
{
    public abstract class ConstructionCostData
    {

        public abstract Dictionary<Type, int> Cost { get; }

    }
}
