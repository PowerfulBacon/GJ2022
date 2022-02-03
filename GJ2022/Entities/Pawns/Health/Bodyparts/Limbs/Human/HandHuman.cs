﻿using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Human
{
    public class HandHuman : Limb
    {
        public HandHuman(Body body, BodySlots slot) : base(body, slot)
        {
        }

        public override BodySlots[] AllowedSlots => new BodySlots[] { BodySlots.SLOT_HAND_LEFT, BodySlots.SLOT_HAND_RIGHT };

        public override LimbFlags DefaultLimbFlags => LimbFlags.NONE;

        public override void SetupOrgans(Pawn pawn, Body body)
        {
            throw new NotImplementedException();
        }
    }
}