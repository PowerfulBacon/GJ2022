﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Human.Bionic;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Pawns.Mobs.Humans
{
    public class BionicHuman : Human
    {

        public BionicHuman(Vector<float> position) : base(position)
        {
            //Replace limbs with bionic limbs
            new ArmBionic(PawnBody, Health.BodySlots.SLOT_ARM_LEFT);
            new ArmBionic(PawnBody, Health.BodySlots.SLOT_ARM_RIGHT);
            new LegBionic(PawnBody, Health.BodySlots.SLOT_LEG_LEFT);
            new LegBionic(PawnBody, Health.BodySlots.SLOT_LEG_RIGHT);
        }

    }
}
