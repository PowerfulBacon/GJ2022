using GJ2022.Game.Construction.Blueprints;

namespace GJ2022.Game.Construction.BlueprintSets
{
    class BuildingBlueprint : BlueprintSet
    {

        public override bool IsRoom => true;

        public override BlueprintDetail BlueprintDetail { get; } = new WallBlueprint();

        public override BlueprintDetail FillerBlueprint { get; } = new FloorBlueprint();

    }
}
