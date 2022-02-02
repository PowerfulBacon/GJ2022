using GJ2022.Entities.Pawns.Health.Bodyparts.Limbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodies.Instances
{
    public class BodyHuman : Body
    {
        public override BodySlots[] BodySlots => new BodySlots[] {
            Health.BodySlots.SLOT_ARM_LEFT,
            Health.BodySlots.SLOT_ARM_RIGHT,
            Health.BodySlots.SLOT_HAND_LEFT,
            Health.BodySlots.SLOT_HAND_RIGHT,
            Health.BodySlots.SLOT_BODY,
            Health.BodySlots.SLOT_HEAD,
            Health.BodySlots.SLOT_LEG_LEFT,
            Health.BodySlots.SLOT_LEG_RIGHT,
            Health.BodySlots.SLOT_FOOT_LEFT,
            Health.BodySlots.SLOT_FOOT_RIGHT,
            Health.BodySlots.SLOT_TAIL,
        };

        protected override void CreateDefaultBodyparts()
        {
            new Bodyparts.Limbs.Human.BodyHuman(this, Health.BodySlots.SLOT_BODY).SetupOrgans(Parent, this);
            new Bodyparts.Limbs.Human.HeadHuman(this, Health.BodySlots.SLOT_BODY).SetupOrgans(Parent, this);
        }
    }
}
