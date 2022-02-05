using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Injuries.Instances.Brain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs.HeadOrgans
{
    public class Brain : Organ
    {

        protected virtual float OxygenConsumptionRate { get; } = 0.02f;
        //The percentage point at which we start taking damage
        protected virtual float OxygenDamageProportion { get; } = 0.6f;
        //Take 0.1 take every tick if 0% oxygen, linearly scales to 0
        protected virtual float OxygenDamageMultiplied { get; } = 0.1f;

        public Brain(Pawn parent, Body body) : base(parent, body)
        { }

        public override OrganFlags DefaultOrganFlags => OrganFlags.ORGAN_PROCESSING;

        public override float ConciousnessFactor => 100;

        public override float MaxHealth => 15;

        public override void OnDestruction()
        {
            //Instant death
            parent.Death("Irreparable damage to the brain");
            //Trigger base stuff
            base.OnDestruction();
        }

        public override void OnPawnLife(float deltaTime)
        {
            float availableOxygen = Math.Min(OxygenConsumptionRate * deltaTime, body.bloodstreamOxygenMoles);
            //Consume oxygen
            body.bloodstreamOxygenMoles -= availableOxygen;
            //Produce carbon dioxide
            body.bloodstreamCarbonDioxideMoles += availableOxygen;
            //Take damage if oxygen is insufficient
            if (availableOxygen < OxygenConsumptionRate * deltaTime * OxygenDamageProportion)
            {
                float damageProportion = 1 - (availableOxygen / OxygenConsumptionRate * deltaTime * OxygenDamageProportion);
                AddInjury(new Hypoxia(damageProportion));
                //TODO: Apply damage
                Log.WriteLine($"A brain just took {damageProportion * OxygenDamageMultiplied * deltaTime} damage!");
            }
        }

    }
}
