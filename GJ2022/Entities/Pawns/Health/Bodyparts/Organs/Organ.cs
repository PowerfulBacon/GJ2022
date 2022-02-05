using GJ2022.Entities.Pawns.Health.Bodies;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs
{
    public abstract class Organ : Bodypart
    {

        //Default organ flags
        public abstract OrganFlags DefaultOrganFlags { get; }

        //Parent of this organ (The pawn we belong to)
        protected Pawn parent;

        //The body we are inside of
        protected Body body;

        //Current flags of the organ, including if it is failing (Fixable) or destroyed (irreperable damage).
        public OrganFlags organFlags;

        public Organ(Pawn parent, Body body)
        {
            //Set the parent
            this.parent = parent;
            this.body = body;
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
