using GJ2022.Game.Construction.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Game.Construction.BlueprintSets
{
    public abstract class BlueprintSet
    {

        public abstract bool IsRoom { get; }

        public abstract BlueprintDetail BlueprintDetail { get; }

        public virtual BlueprintDetail FillerBlueprint { get; }

    }
}
