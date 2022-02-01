using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs.Head
{
    public class Eye : Organ
    {

        public Eye(Pawn parent) : base(parent)
        { }

        public override OrganFlags DefaultOrganFlags => OrganFlags.NONE;

        public override float VisionFactor => 50;

    }
}
