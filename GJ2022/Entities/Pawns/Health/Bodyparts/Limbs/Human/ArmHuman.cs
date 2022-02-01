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
        public override BodySlots[] AllowedSlots => new BodySlots[] { BodySlots.SLOT_ARM_LEFT, BodySlots.SLOT_ARM_RIGHT };

        public override Organ[] ContainedOrgans => new Organ[];

        public override LimbFlags DefaultLimbFlags => LimbFlags.NONE;
    }
}
