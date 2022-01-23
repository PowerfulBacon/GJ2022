using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Game.Construction
{
    public abstract class BlueprintDetail
    {

        public const int MAX_BLUEPRINT_LAYER = 2;

        //The name of the blueprint
        public abstract string Name { get; }

        //Is the blueprint a room or a line?
        public abstract bool IsRoom { get; }

        //The layer of the blueprint (floors and furnature can overlap)
        public abstract int BlueprintLayer { get; }

        //The texture of the border
        public abstract string BorderTexture { get; }

        //The type of the blueprint border
        public abstract Type BorderType { get; }

        //The type of the blueprint filler (If its a room)
        public virtual Type FloorType { get; } = null;

        //The floor texture
        public virtual string FloorTexture { get; } = "error";

    }
}
