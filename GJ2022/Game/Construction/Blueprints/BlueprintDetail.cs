using GJ2022.Entities.Blueprints;
using GJ2022.Game.Construction.Cost;
using System;

namespace GJ2022.Game.Construction.Blueprints
{
    public class BlueprintDetail
    {

        public const int MAX_BLUEPRINT_LAYER = 1;

        //The type of the blueprint being created
        public Type BlueprintType { get; } = typeof(Blueprint);

        //Construction cost of the blueprint
        public ConstructionCostData Cost { get; }

        //The layer of the blueprint (floors and furnature can overlap)
        public int BlueprintLayer { get; }

        //The texture of the border
        public string Texture { get; }

        //The type of the blueprint border
        public string CreatedDef { get; }

        //Priority of the wall
        public int Priority { get; }

        //Is the blueprint built instantly
        public bool Instant { get; }

        public BlueprintDetail(Type blueprintType, ConstructionCostData cost, int blueprintLayer, string texture, string createdDef, int priority, bool instant)
        {
            BlueprintType = blueprintType;
            Cost = cost;
            BlueprintLayer = blueprintLayer;
            Texture = texture;
            CreatedDef = createdDef;
            Priority = priority;
            Instant = instant;
        }
    }
}
