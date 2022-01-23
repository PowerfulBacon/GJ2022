using GJ2022.Entities.Blueprints;
using System;

namespace GJ2022.Game.Construction
{
    public abstract class BlueprintDetail
    {

        public const int MAX_BLUEPRINT_LAYER = 1;

        //The name of the blueprint
        public abstract string Name { get; }

        //Is the blueprint a room or a line?
        public abstract bool IsRoom { get; }

        //The type of the blueprint being created
        public virtual Type BlueprintType { get; } = typeof(Blueprint);

        //The layer of the blueprint (floors and furnature can overlap)
        public abstract int BlueprintLayer { get; }

        //The texture of the border
        public abstract string BorderTexture { get; }

        //The type of the blueprint border
        public abstract Type BorderType { get; }

        //Priority of the wall
        public abstract int BorderPriority { get; }

        //The type of the blueprint filler (If its a room)
        public virtual Type FloorType { get; } = null;

        //The floor texture
        public virtual string FloorTexture { get; } = "error";

        //Priority of the floor
        public virtual int FloorPriority { get; } = 0;

    }
}
