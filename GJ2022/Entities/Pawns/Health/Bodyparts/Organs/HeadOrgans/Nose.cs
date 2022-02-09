using GJ2022.Entities.Items.Clothing;
using GJ2022.Entities.Pawns.Health.Bodies;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs.HeadOrgans
{
    public class Nose : Organ
    {
        public Nose(Pawn parent, Body body) : base(parent, body)
        { }

        public override OrganFlags DefaultOrganFlags => OrganFlags.NONE;

        public override float MaxHealth => 15;

        public override BodyCoverFlags CoverFlags => BodyCoverFlags.COVER_HEAD;
    }
}
