using GJ2022.Entities.Pawns.Health.Bodies;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs
{
    public abstract class Organ : Bodypart
    {

        //========================
        // Organ Factors
        //========================
        //The amount of something a pawn will receive from having this.
        //As the organ loses health, the factor decreases linearly
        //For example:
        //An ear would have hearing of 50, meaning when its destroyed it provides 0 hearing.
        //An implant could have a factor of 10, meaning when inserted the stat is improved by 10,
        //allowing it to become 110 for example, when destroyed it will go down by the bonus it gives.

        //Conciousness
        public virtual float ConciousnessFactor { get; } = 0;

        //Movement
        public virtual float MovementFactor { get; } = 0;

        //Manipulation
        public virtual float ManipulationFactor { get; } = 0;

        //Vision
        public virtual float VisionFactor { get; } = 0;

        //Hearing
        public virtual float HearingFactor { get; } = 0;

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
            //Set the default organ flags
            organFlags = DefaultOrganFlags;
        }

        public virtual void OnDestruction()
        {
            //Set destroyed flags
            organFlags |= OrganFlags.ORGAN_DESTROYED;
            organFlags |= OrganFlags.ORGAN_FAILING;
        }

        public virtual void OnPawnLife()
        {
            throw new System.NotImplementedException();
        }

    }
}
