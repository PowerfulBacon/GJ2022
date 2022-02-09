using GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Limbs;

namespace GJ2022.Entities.Pawns.Health.Bodies.Instances
{
    public class BodyHuman : Body
    {

        public override BodySlots[] BodySlots => new BodySlots[] {
            Health.BodySlots.SLOT_ARM_LEFT,
            Health.BodySlots.SLOT_ARM_RIGHT,
            Health.BodySlots.SLOT_BODY,
            Health.BodySlots.SLOT_HEAD,
            Health.BodySlots.SLOT_LEG_LEFT,
            Health.BodySlots.SLOT_LEG_RIGHT,
            Health.BodySlots.SLOT_TAIL,
        };

        public override bool HasGender => true;

        public override bool SupportsLimbOverlays => true;

        protected override void CreateDefaultBodyparts()
        {
            new Bodyparts.Limbs.Limbs.BodyHuman(this, Health.BodySlots.SLOT_BODY);
            new HeadHuman(this, Health.BodySlots.SLOT_HEAD);
            new ArmHuman(this, Health.BodySlots.SLOT_ARM_LEFT);
            new ArmHuman(this, Health.BodySlots.SLOT_ARM_RIGHT);
            new LegHuman(this, Health.BodySlots.SLOT_LEG_LEFT);
            new LegHuman(this, Health.BodySlots.SLOT_LEG_RIGHT);
        }
    }
}
