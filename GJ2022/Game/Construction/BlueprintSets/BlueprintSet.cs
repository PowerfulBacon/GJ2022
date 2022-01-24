using GJ2022.Game.Construction.Blueprints;

namespace GJ2022.Game.Construction.BlueprintSets
{
    public abstract class BlueprintSet
    {

        public abstract bool IsRoom { get; }

        public abstract BlueprintDetail BlueprintDetail { get; }

        public virtual BlueprintDetail FillerBlueprint { get; }

    }
}
