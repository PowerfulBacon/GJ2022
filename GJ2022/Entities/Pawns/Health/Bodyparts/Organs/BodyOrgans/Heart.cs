﻿using GJ2022.Atmospherics.Gasses;
using GJ2022.Entities.Items.Clothing;
using GJ2022.Entities.Pawns.Health.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs.BodyOrgans
{
    public class Heart : Organ
    {
        public Heart(Pawn parent, Body body) : base(parent, body)
        { }

        public override OrganFlags DefaultOrganFlags => OrganFlags.ORGAN_PROCESSING;

        public override float MaxHealth => 25;

        public override void OnPawnLife(float deltaTime)
        {
            //1 mole of oxygen is about 122 kPa at room temp.
            //this is just an arbitary number to stop pawns getting infinite oxygen and the lungs are always fine
            float maximumBodyOxygen = 0.05f;
            //Take oxygen from the internal atmosphere and put it into the body.
            float transferedMoles = Math.Max(Math.Min(Body.internalAtmosphere.GetMoles(Oxygen.Singleton), maximumBodyOxygen) - Body.bloodstreamOxygenMoles, 0);
            Body.bloodstreamOxygenMoles += transferedMoles;
            Body.internalAtmosphere.SetMoles(Oxygen.Singleton, Body.internalAtmosphere.GetMoles(Oxygen.Singleton) - transferedMoles);
            //Put carbon dioxide from the body into the bodies internal atmosphere.
            Body.internalAtmosphere.SetMoles(CarbonDioxide.Singleton, Body.internalAtmosphere.GetMoles(CarbonDioxide.Singleton) + Body.bloodstreamCarbonDioxideMoles);
            Body.bloodstreamCarbonDioxideMoles = 0;
        }
    }
}
