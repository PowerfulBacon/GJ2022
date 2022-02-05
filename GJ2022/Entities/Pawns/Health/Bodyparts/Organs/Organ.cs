using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Bodyparts.Limbs;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs
{
    public abstract class Organ : Bodypart
    {

        //Default organ flags
        public abstract OrganFlags DefaultOrganFlags { get; }

        //Current flags of the organ, including if it is failing (Fixable) or destroyed (irreperable damage).
        public OrganFlags organFlags;

        //The limb we are contained within TODO
        public Limb ContainingLimb { get; private set; }

        public Organ(Pawn parent, Body body) : base(body)
        {
            //Set the default organ flags
            organFlags = DefaultOrganFlags;
            //Set processing
            if ((organFlags & OrganFlags.ORGAN_PROCESSING) != 0)
            {
                body.processingOrgans.Add(this);
            }
        }

        public override void OnDestruction()
        {
            //Set destroyed flags
            organFlags |= OrganFlags.ORGAN_DESTROYED;
            organFlags |= OrganFlags.ORGAN_FAILING;
        }

        public virtual void OnPawnLife(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

    }
}
