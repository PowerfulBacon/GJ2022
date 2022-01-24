using GJ2022.Entities.Blueprints;
using GJ2022.Game.Construction.Cost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Game.Construction.Blueprints
{
    public abstract class BlueprintDetail
    {

        public const int MAX_BLUEPRINT_LAYER = 1;

        //The type of the blueprint being created
        public virtual Type BlueprintType { get; } = typeof(Blueprint);

        //Construction cost of the blueprint
        public abstract ConstructionCostData Cost { get; }

        //The layer of the blueprint (floors and furnature can overlap)
        public abstract int BlueprintLayer { get; }

        //The texture of the border
        public abstract string Texture { get; }

        //The type of the blueprint border
        public abstract Type CreatedType { get; }

        //Priority of the wall
        public abstract int Priority { get; }

    }
}
