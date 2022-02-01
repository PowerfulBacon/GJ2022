using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs.HeadOrgans
{
    public class Ear : Organ
    {

        public Ear(Pawn parent) : base(parent)
        { }

        public override OrganFlags DefaultOrganFlags => OrganFlags.NONE;

        public override float HearingFactor => 50;

    }
}
