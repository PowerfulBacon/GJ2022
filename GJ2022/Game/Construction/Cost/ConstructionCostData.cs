﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Game.Construction.Cost
{
    public abstract class ConstructionCostData
    {

        public abstract Dictionary<Type, int> Cost { get; }

    }
}
