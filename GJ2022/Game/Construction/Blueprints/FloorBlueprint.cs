using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Entities.Blueprints;
using GJ2022.Entities.Turfs.Standard.Floors;
using GJ2022.Game.Construction.Cost;

namespace GJ2022.Game.Construction.Blueprints
{
    class FloorBlueprint : BlueprintDetail
    {

        public override ConstructionCostData Cost { get; } = new FloorCost();

        public override int BlueprintLayer => 1;

        public override string Texture => "plating";

        public override Type CreatedType => typeof(Plating);

        public override int Priority => 2;

        public override Type BlueprintType => typeof(TurfBlueprint);
    }
}
