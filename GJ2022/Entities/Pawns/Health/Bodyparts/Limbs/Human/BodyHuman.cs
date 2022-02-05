using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs.BodyOrgans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Human
{
    public class BodyHuman : Limb
    {
        public BodyHuman(Body body, BodySlots slot) : base(body, slot)
        {
        }

        public override BodySlots[] AllowedSlots => new BodySlots[] { BodySlots.SLOT_BODY };

        public override LimbFlags DefaultLimbFlags => LimbFlags.CRITICAL_LIMB | LimbFlags.NO_REMOVAL;

        public override float MaxHealth => 60;

        public override void SetupOrgans(Pawn pawn, Body body)
        {
            containedOrgans.Add(new Heart(pawn, body));
            containedOrgans.Add(new Liver(pawn, body));
            containedOrgans.Add(new Lung(pawn, body));
            containedOrgans.Add(new Stomach(pawn, body));
        }

    }
}
