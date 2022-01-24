using GJ2022.Entities.Items.Stacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
