using GJ2022.Entities.Blueprints;
using GJ2022.Entities.Turfs.Standard.Floors;
using GJ2022.Entities.Turfs.Standard.Solids;
using System;

namespace GJ2022.Game.Construction.Walls
{
    public class FoundationBlueprint : BlueprintDetail
    {

        public override string Name => "Metal Foundations";

        public override bool IsRoom => true;

        public override Type BorderType => typeof(Wall);

        public override Type FloorType => typeof(Plating);

        public override string BorderTexture => "wall";

        public override string FloorTexture => "plating";

        public override int BlueprintLayer => 0;

        public override Type BlueprintType => typeof(TurfBlueprint);

        public override int BorderPriority { get; } = 1;

        public override int FloorPriority { get; } = 2;
    }
}
