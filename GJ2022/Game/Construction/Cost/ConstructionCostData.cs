using GJ2022.EntityLoading.XmlDataStructures;
using System;
using System.Collections.Generic;

namespace GJ2022.Game.Construction.Cost
{
    public class ConstructionCostData
    {

        public Dictionary<EntityDef, int> Cost { get; } = new Dictionary<EntityDef, int>();

    }
}
