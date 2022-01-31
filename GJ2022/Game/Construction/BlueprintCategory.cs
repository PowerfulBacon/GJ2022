using GJ2022.Game.Construction.BlueprintSets;
using System.Collections.Generic;

namespace GJ2022.Game.Construction
{
    public class BlueprintCategory
    {

        public string Name { get; }

        public List<BlueprintSet> Contents { get; }

        public BlueprintCategory(string name)
        {
            Name = name;
            Contents = new List<BlueprintSet>();
        }
    }
}
