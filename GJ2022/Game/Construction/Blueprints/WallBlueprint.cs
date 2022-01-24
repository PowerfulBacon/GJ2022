using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Entities.Blueprints;
using GJ2022.Entities.Turfs.Standard.Solids;
using GJ2022.Game.Construction.Cost;

namespace GJ2022.Game.Construction.Blueprints
{
    class WallBlueprint : BlueprintDetail
    {

        public override ConstructionCostData Cost { get; } = new WallCost();

        public override int BlueprintLayer => 1;

        public override string Texture => "wall";

        public override Type CreatedType => typeof(Wall);

        public override int Priority => 1;

        public override Type BlueprintType => typeof(TurfBlueprint);

    }
}
