﻿using GJ2022.Atmospherics;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs;
using System.Collections.Generic;

namespace GJ2022.Entities.Pawns.Health.Bodies
{
    public abstract class Body
    {

        //List of the name of the body slots this body uses.
        public abstract BodySlots[] BodySlots { get; }

        //Internal atmosphere of the lungs
        //Lungs handle moving gasses from the atmosphere into here
        public Atmosphere internalAtmosphere;

        //Heart handles moving gasses from internal atmosphere into the bloodstream
        //(Not realistic in code, but an optimisation the players won't see)
        //Blood stream carbon dioxide
        public float bloodstreamCarbonDioxideMoles;

        //Blood stream oxygen
        public float bloodstreamOxygenMoles;

        public List<Organ> processingOrgans = new List<Organ>();

        public Pawn Parent { get; private set; }

        /// <summary>
        /// Setup the body and its internal atmosphere
        /// </summary>
        public Body()
        {
            internalAtmosphere = new Atmosphere(AtmosphericConstants.IDEAL_TEMPERATURE);
            //Lungs hold 6 litres of air
            internalAtmosphere.SetVolume(20);
        }

        public void SetupBody(Pawn parent)
        {
            Parent = parent;
            CreateDefaultBodyparts();
        }

        protected abstract void CreateDefaultBodyparts();

        public void ProcessBody(float deltaTime)
        {
            Log.WriteLine($"Processing {processingOrgans.Count} organs");
            //Process all processing organs
            for (int i = processingOrgans.Count - 1; i >= 0; i--)
            {
                //Check the organ
                Organ organ = processingOrgans[i];
                //Check for failure
                if ((organ.organFlags & OrganFlags.ORGAN_FAILING | OrganFlags.ORGAN_DESTROYED) != 0)
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
