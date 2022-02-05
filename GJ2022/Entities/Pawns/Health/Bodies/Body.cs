using GJ2022.Atmospherics;
using GJ2022.Entities.Pawns.Health.Bodyparts;
using GJ2022.Entities.Pawns.Health.Bodyparts.Limbs;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs;
using GJ2022.Rendering.RenderSystems.Renderables;
using System.Collections.Generic;

namespace GJ2022.Entities.Pawns.Health.Bodies
{
    public abstract class Body
    {

        //List of the name of the body slots this body uses.
        public abstract BodySlots[] BodySlots { get; }

        //Dictionary of bodyparts inside this body
        public Dictionary<BodySlots, Limb> InsertedLimbs { get; } = new Dictionary<BodySlots, Limb>();

        //Internal atmosphere of the lungs
        //Lungs handle moving gasses from the atmosphere into here
        public Atmosphere internalAtmosphere;

        //Heart handles moving gasses from internal atmosphere into the bloodstream
        //(Not realistic in code, but an optimisation the players won't see)
        //Blood stream carbon dioxide
        public float bloodstreamCarbonDioxideMoles;

        //Blood stream oxygen
        public float bloodstreamOxygenMoles;

        //The organs being processed by this body
        public List<Organ> processingOrgans = new List<Organ>();

        //The pawn we are attached to
        public Pawn Parent { get; private set; }

        //Stats
        public float Conciousness   { get; internal set; } = 0;
        public float Movement       { get; internal set; } = 0;
        public float Manipulation   { get; internal set; } = 0;
        public float Vision         { get; internal set; } = 0;
        public float Hearing        { get; internal set; } = 0;

        /// <summary>
        /// Setup the body and its internal atmosphere
        /// </summary>
        public Body()
        {
            internalAtmosphere = new Atmosphere(AtmosphericConstants.IDEAL_TEMPERATURE);
            //Lungs hold 6 litres of air
            internalAtmosphere.SetVolume(20);
            //Set the inserted bodypart dictionary
            foreach (BodySlots slot in BodySlots)
            {
                InsertedLimbs.Add(slot, null);
            }
        }

        public void SetupBody(Pawn parent)
        {
            Parent = parent;
            CreateDefaultBodyparts();
        }

        protected abstract void CreateDefaultBodyparts();

        protected void SetupAndInsertLimb(Limb limb, BodySlots slot)
        {
            limb.SetupOrgans(Parent, this);
            limb.Insert(this, slot);
        }

        public void ProcessBody(float deltaTime)
        {
            //Process pressure damage
            
            //Process all processing organs
            for (int i = processingOrgans.Count - 1; i >= 0; i--)
            {
                //Check the organ
                Organ organ = processingOrgans[i];
                //Check for failure
                if ((organ.organFlags & (OrganFlags.ORGAN_FAILING | OrganFlags.ORGAN_DESTROYED)) != 0)
                {
                    continue;
                }
                //Check for processing
                if ((organ.organFlags & OrganFlags.ORGAN_PROCESSING) == 0)
                {
                    processingOrgans.RemoveAt(i);
                    continue;
                }
                //Do process
                organ.OnPawnLife(deltaTime);
            }
        }

    }
}
