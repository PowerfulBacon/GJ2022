﻿using GJ2022.Entities.Pawns.Health.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs.BodyOrgans
{
    public class Lung : Organ
    {
        public Lung(Pawn parent, Body body) : base(parent, body)
        { }

        public override OrganFlags DefaultOrganFlags => OrganFlags.NONE;

        /// <summary>
        /// Increase volume of the lungs to get air in the atmosphere.
        /// Move gasses into the blood stream
        /// </summary>
        public override void OnPawnLife()
        {
            
        }
    }
}
