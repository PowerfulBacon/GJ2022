using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

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

        //Organs that are inside of us
        public List<Organ> containedOrgans = new List<Organ>();

        //Flags of this limb
        public abstract LimbFlags DefaultLimbFlags { get; }

        //The parent body
        public Body ParentBody { get; private set; }

        //Slot we are inserted into
        public BodySlots InsertedSlot { get; private set; }

        //The default limb flags of this limb
        public LimbFlags limbFlags;

        public Limb(Body body, BodySlots slot)
        {
            limbFlags = DefaultLimbFlags;
            Insert(body, slot);
        }

        public bool Insert(Body body, BodySlots slot)
        {
            //Remove the old organ
            if (body.InsertedBodyparts[slot] != null)
            {
                //We cannot remove the old bodypart
                if (!body.InsertedBodyparts[slot].Remove())
                {
                    return false;
                }
            }
            //Insert the organ
            ParentBody = body;
            body.InsertedBodyparts[slot] = this;
            return true;
        }

        public override bool Remove()
        {
            //Remove from the body
            ParentBody.InsertedBodyparts[InsertedSlot] = null;
            return base.Remove();
        }

        public abstract void SetupOrgans(Pawn pawn, Body body);

    }

}
