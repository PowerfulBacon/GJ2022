using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs.HeadOrgans
{
    public class Brain : Organ
    {

        public Brain(Pawn parent) : base(parent)
        { }

        public override OrganFlags DefaultOrganFlags => OrganFlags.NONE;

        public override float ConciousnessFactor => 100;

        public override void OnDestruction()
        {
            //Instant death
            parent.Death("Irreparable damage to the brain");
            //Trigger base stuff
            base.OnDestruction();
        }

    }
}
