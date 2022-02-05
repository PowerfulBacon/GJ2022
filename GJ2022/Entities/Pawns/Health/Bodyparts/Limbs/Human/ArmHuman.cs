using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Human
{
    public class ArmHuman : Limb
    {
        public ArmHuman(Body body, BodySlots slot) : base(body, slot)
        {
        }

        public override BodySlots[] AllowedSlots => new BodySlots[] { BodySlots.SLOT_ARM_LEFT, BodySlots.SLOT_ARM_RIGHT };

        public override LimbFlags DefaultLimbFlags => LimbFlags.NONE;

        public override float MaxHealth => 35;

        public override float ManipulationFactor => 50;

        public override void SetupOrgans(Pawn pawn, Body body)
        {
            throw new NotImplementedException();
        }

        public override void OnDestruction()
        {
            //Destroy the associated hand
        }
    }
}
