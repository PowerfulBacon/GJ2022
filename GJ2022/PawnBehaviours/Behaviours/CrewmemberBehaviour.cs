using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours.PawnActions;
using System.Collections.Generic;

namespace GJ2022.PawnBehaviours.Behaviours
{
    class CrewmemberBehaviour : PawnBehaviour
    {

        public CrewmemberBehaviour(Pawn owner) : base(owner)
        { }

        public override Dictionary<PawnAction, double> Actions { get; } = new Dictionary<PawnAction, double>()
        {
            { new HaulItems(), 0 },
            { new DeliverMaterialsToBlueprints(), 0 },
            { new ConstructBlueprints(), 0 },
            { new MineAction(), 0 },
            { new SmeltOre(), 0 },
            { new ReturnToSpawn(), 0 }
        };

    }
}
