using GJ2022.Entities.Items.Clothing;
using GJ2022.Entities.Pawns.Health.Bodies;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Limbs.Corgi
{
    public class LegCorgi : Limb
    {

        public LegCorgi(Body body, BodySlots slot) : base(body, slot)
        {
        }

        //Allow these to be put inside humans?
        public override BodySlots[] AllowedSlots => new BodySlots[] {
            BodySlots.SLOT_LEG_BACK_LEFT,
            BodySlots.SLOT_LEG_BACK_RIGHT,
            BodySlots.SLOT_LEG_FRONT_LEFT,
            BodySlots.SLOT_LEG_FRONT_RIGHT
        };

        public override LimbFlags DefaultLimbFlags => LimbFlags.NONE;

        public override float MaxHealth => 10;

        public override float MovementFactor => 25;

        public override float HighPressureDamage => 200;

        public override float LowPressureDamage => 20;

        public override BodyCoverFlags CoverFlags => BodyCoverFlags.COVER_LEGS | BodyCoverFlags.COVER_ARMS;

        public override void SetupOrgans(Pawn pawn, Body body)
        {
            //TODO
            return;
        }

    }
}
