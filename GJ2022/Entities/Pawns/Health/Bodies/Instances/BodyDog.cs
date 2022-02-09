using GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Limbs.Corgi;

namespace GJ2022.Entities.Pawns.Health.Bodies.Instances
{
    public class BodyDog : Body
    {
        public override BodySlots[] BodySlots => new BodySlots[] {
            Health.BodySlots.SLOT_BODY,
            Health.BodySlots.SLOT_HEAD,
            Health.BodySlots.SLOT_TAIL,
            Health.BodySlots.SLOT_LEG_BACK_LEFT,
            Health.BodySlots.SLOT_LEG_BACK_RIGHT,
            Health.BodySlots.SLOT_LEG_FRONT_LEFT,
            Health.BodySlots.SLOT_LEG_FRONT_RIGHT,
        };

        public override bool HasGender => true;

        protected override void CreateDefaultBodyparts()
        {
            new BodyCorgi(this, Health.BodySlots.SLOT_BODY);
            new HeadCorgi(this, Health.BodySlots.SLOT_HEAD);
            new LegCorgi(this, Health.BodySlots.SLOT_LEG_BACK_LEFT);
            new LegCorgi(this, Health.BodySlots.SLOT_LEG_BACK_RIGHT);
            new LegCorgi(this, Health.BodySlots.SLOT_LEG_FRONT_LEFT);
            new LegCorgi(this, Health.BodySlots.SLOT_LEG_FRONT_RIGHT);
        }
    }
}
