using GJ2022.Entities.Items.Stacks;
using System;
using System.Collections.Generic;

namespace GJ2022.Game.Construction.Cost
{
    class WallCost : ConstructionCostData
    {

        public override Dictionary<Type, int> Cost { get; } = new Dictionary<Type, int>()
        {
            { typeof(Iron), 5 }
        };

    }
}
