using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Game.Construction
{
    public abstract class BlueprintDetail
    {

        //The name of the blueprint
        public abstract string Name { get; }

        //Is the blueprint a room or a line?
        public abstract bool IsRoom { get; }

        //The type of the blueprint border
        public abstract Type BorderType { get; }

        //The type of the blueprint filler (If its a room)
        public virtual Type FloorType { get; } = null;

    }
}
