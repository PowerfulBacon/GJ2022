using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Entities.Pawns.Health.Bodies;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Human
{
    public class LegHuman : Limb
    {
        public LegHuman(Body body, BodySlots slot) : base(body, slot)
        {
        }

        public override BodySlots[] AllowedSlots => new BodySlots[] { BodySlots.SLOT_LEG_LEFT, BodySlots.SLOT_LEG_RIGHT };

        public override LimbFlags DefaultLimbFlags => LimbFlags.NONE;

        public override float MaxHealth => 35;

        public override void SetupOrgans(Pawn pawn, Body body)
        {
            throw new NotImplementedException();
        }
    }
}
