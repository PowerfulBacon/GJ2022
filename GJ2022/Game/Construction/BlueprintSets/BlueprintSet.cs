using GJ2022.Game.Construction.Blueprints;

namespace GJ2022.Game.Construction.BlueprintSets
{
    public class BlueprintSet
    {

        public bool IsRoom { get; }

        public string Name { get; }

        public BlueprintDetail BlueprintDetail { get; }

        public BlueprintDetail FillerBlueprint { get; }

        public BlueprintSet(string name, bool isRoom, BlueprintDetail blueprintDetail, BlueprintDetail fillerBlueprint)
        {
            Name = name;
            IsRoom = isRoom;
            BlueprintDetail = blueprintDetail;
            FillerBlueprint = fillerBlueprint;
        }
    }
}
