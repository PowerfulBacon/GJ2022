using GJ2022.Entities.Pawns.Health.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs.HeadOrgans
{
    public class Eye : Organ
    {

        public Eye(Pawn parent, Body body) : base(parent, body)
        { }

        public override OrganFlags DefaultOrganFlags => OrganFlags.NONE;

        public override float VisionFactor => 50;

        public override float MaxHealth => 10;

    }
}
