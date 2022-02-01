using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs.Body
{
    public class Stomach : Organ
    {

        public Stomach(Pawn parent) : base(parent)
        { }

        public override OrganFlags DefaultOrganFlags => OrganFlags.NONE;

    }
}
