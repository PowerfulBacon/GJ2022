using GJ2022.Entities.Pawns.Health.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs.BodyOrgans
{
    public class Stomach : Organ
    {

        public Stomach(Pawn parent, Body body) : base(parent, body)
        { }

        public override OrganFlags DefaultOrganFlags => OrganFlags.NONE;

        public override float MaxHealth => 20;
    }
}
