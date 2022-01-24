using GJ2022.Entities.Items.Stacks;
using System;
using System.Collections.Generic;

namespace GJ2022.Game.Construction.Cost
{
    class FloorCost : ConstructionCostData
    {

        public override Dictionary<Type, int> Cost { get; } = new Dictionary<Type, int>()
        {
            { typeof(Iron), 2 }
        };

    }
}
