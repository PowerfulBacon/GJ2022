using GJ2022.Entities.Items.Clothing;
using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Injuries;
using GJ2022.Rendering.RenderSystems.Renderables;
using System.Collections.Generic;

namespace GJ2022.Entities.Pawns.Health.Bodyparts
{
    public abstract class Bodypart
    {

        //The body we are inside of
        protected Body Body { get; set; }

        //========================
        // Bodypart Factors
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

        //Max health of the bodypart
        public abstract float MaxHealth { get; }

        //The parts a peice of clothing must cover in order to protect this
        public virtual BodyCoverFlags CoverFlags { get; }

        //Current bodypart health
        public float Health { get; private set; } = 0;

        //List of inflicting injuries
        private List<Injury> injuries = new List<Injury>();

        public Bodypart(Body body)
        {
            Body = body;
            Health = MaxHealth;
        }

        //TODO: Make this apply damage and the special effects of injuries
        public void AddInjury(Injury injury)
        {
            //Calculate new damage
            Health -= injury.Damage;
            //Update pain
            Body.AdjustPain(injury.PainPerDamage * injury.Damage);
            //Check for destruction
            if (Health <= 0)
            {
                OnDestruction();
            }
            //Check uniqueness
            if (injury.Unique)
            {
                injuries.Add(injury);
                //Update the damage overlays
                UpdateDamageOverlays(Body.Parent.Renderable);
                return;
            }
            //Check stacking
            for (int i = injuries.Count - 1; i >= 0; i--)
            {
                if (!injuries[i].Unique && injuries.GetType() == injury.GetType())
                {
                    injuries[i].Damage += injury.Damage;
                    //Update the damage overlays
                    UpdateDamageOverlays(Body.Parent.Renderable);
                    return;
                }
            }
            injuries.Add(injury);
            //Update the damage overlays
            UpdateDamageOverlays(Body.Parent.Renderable);
        }

        //Remove from body
        public virtual bool Remove()
        {
            //Remove the bodies overall stats
            //TODO: Account for damage
            Body.Conciousness -= ConciousnessFactor;
            Body.Manipulation -= ManipulationFactor;
            Body.Movement -= MovementFactor;
            Body.Hearing -= HearingFactor;
            Body.Vision -= VisionFactor;
            //Remove overlays
            if(Body.Parent.Renderable != null)
                RemoveOverlay(Body.Parent.Renderable);
            return true;
        }

        //Called when the organ is destroyed
        public virtual void OnDestruction()
        { }

        public virtual void AddOverlay(Renderable renderable)
        { }

        public virtual void RemoveOverlay(Renderable renderable)
        { }

        public virtual void UpdateDamageOverlays(Renderable renderable)
        { }

        public bool IsCovered(BodyCoverFlags coveredFlags)
        {
            return (CoverFlags & coveredFlags) == CoverFlags;
        }

    }
}
