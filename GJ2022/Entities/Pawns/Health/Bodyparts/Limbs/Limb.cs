using GJ2022.Entities.Pawns.Health.Bodyparts.Organs;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs
{
    public abstract class Limb : Bodypart
    {

        //An array containing all the allowed slots this limb can go in.
        //(Legs can go into leg slots etc.)
        public abstract BodySlots[] AllowedSlots { get; }

        //The name of the texture that gets applied to humans if this limb is applied to them.
        //Ignore if this can't be applied to humans
        public virtual string HumanOverlayTextureId { get; } = null;

        //The colour of the limb when applied to a human.
        //(A corgi leg applied to a human looks like a human leg but with corgi fur colour)
        public virtual Colour? HumanOverlayColour { get; } = null;

        public abstract Organ[] ContainedOrgnas { get; }

    }

}
