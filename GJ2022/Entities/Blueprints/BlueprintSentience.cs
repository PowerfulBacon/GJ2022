using GJ2022.Entities.Items;
using GJ2022.Entities.Pawns;
using GJ2022.Game.Construction.Blueprints;
using GJ2022.PawnBehaviours.Behaviours;
using GJ2022.Utility.MathConstructs;
using System;

namespace GJ2022.Entities.Blueprints
{
    public class BlueprintSentience : Blueprint
    {

        public BlueprintSentience(Vector<float> position, BlueprintDetail detail) : base(position, detail)
        { }

        public override void Complete()
        {
            //Clear and delete all contents
            foreach (Item item in contents)
            {
                item.Destroy();
            }
            contents.Clear();
            //Create an instance of the thingy
            Pawn pawn = Activator.CreateInstance(BlueprintDetail.CreatedType, Position) as Pawn;
            new CrewmemberBehaviour(pawn);
            //Destroy the blueprint
            Destroy();
        }

    }
}
