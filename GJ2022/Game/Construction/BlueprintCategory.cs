using GJ2022.Game.Construction.BlueprintSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
