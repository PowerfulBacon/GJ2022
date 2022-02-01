﻿using GJ2022.Entities.Pawns.Health.Bodyparts.Organs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Human
{
    public class BodyHuman : Limb
    {
        public override BodySlots[] AllowedSlots => new BodySlots[] { BodySlots.SLOT_BODY };

        public override Organ[] ContainedOrgans => new Organ[];

        public override LimbFlags DefaultLimbFlags => LimbFlags.CRITICAL_LIMB | LimbFlags.NO_REMOVAL;

    }
}
