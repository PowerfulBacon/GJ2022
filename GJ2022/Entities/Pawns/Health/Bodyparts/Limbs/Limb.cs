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

        //The colour of the limb when applied to a human.
        //(A corgi leg applied to a human looks like a human leg but with corgi fur colour)
        //TODO
        public virtual Colour? HumanOverlayColour { get; } = null;

        //Organs that are inside of us
        public List<Organ> containedOrgans = new List<Organ>();

        //Flags of this limb
        public abstract LimbFlags DefaultLimbFlags { get; }

        //Slot we are inserted into
        public BodySlots InsertedSlot { get; private set; }

        //The default limb flags of this limb
        public LimbFlags limbFlags;

        //Maximum allowed pressure
        public abstract float HighPressureDamage { get; }
        public abstract float LowPressureDamage { get; }

        public Limb(Body body, BodySlots slot) : base(body)
        {
            limbFlags = DefaultLimbFlags;
            //Setup the internal organs of this bodypart
            SetupOrgans(body.Parent, body);
            //Insert it into the body provided
            Insert(body, slot);
        }

        public bool Insert(Body body, BodySlots slot)
        {
            //Remove the old organ
            if (body.InsertedLimbs[slot] != null)
            {
                //We cannot remove the old bodypart
                if (!body.InsertedLimbs[slot].Remove())
                {
                    return false;
                }
            }
            //Insert the limb
            body.InsertedLimbs[slot] = this;
            //Update body
            body.Conciousness += ConciousnessFactor;
            body.Manipulation += ManipulationFactor;
            body.Movement += MovementFactor;
            body.Hearing += HearingFactor;
            body.Vision += VisionFactor;
            //Set tje omserted ;pcatopm
            InsertedSlot = slot;
            //Overlay handling
            if (body.Parent.UsesLimbOverlays)
                AddOverlay(body.Parent.Renderable);
            return true;
        }

        public override bool Remove()
        {
            if (!base.Remove())
                return false;
            //Remove from the body
            Body.InsertedLimbs[InsertedSlot] = null;
            //We can be garbage collected at this point
            //TODO: Drop an organ item
            Body = null;
            return true;
        }

        public abstract void SetupOrgans(Pawn pawn, Body body);

    }

}
