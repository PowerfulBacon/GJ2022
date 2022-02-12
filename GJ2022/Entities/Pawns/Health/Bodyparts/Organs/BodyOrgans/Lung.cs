using GJ2022.Atmospherics;
using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Injuries.Instances.Generic;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs.BodyOrgans
{
    public class Lung : Organ
    {

        public Lung(Pawn parent, Body body) : base(parent, body)
        { }

        public override OrganFlags DefaultOrganFlags => OrganFlags.ORGAN_PROCESSING;

        public override float MaxHealth => 20;

        /// <summary>
        /// Increase volume of the lungs to get air in the atmosphere.
        /// Move gasses into the blood stream
        /// </summary>
        public override void OnPawnLife(float deltaTime)
        {
            Atmosphere atmosphere = Body.Parent.GetBreathSource();
            //No atmosphere = vent lungs
            if (atmosphere == null)
            {
                Body.internalAtmosphere.ClearGasses();
                return;
            }
            //Equalize gasses in lungs with the gasses in the atmosphere.
            Body.internalAtmosphere.Equalize(atmosphere);
            ProcessLungDamage(deltaTime, atmosphere);
        }

        private void ProcessLungDamage(float deltaTime, Atmosphere atmosphere)
        {
            //If the temperature is too extreme, take damage (60 degrees)
            if (atmosphere.KelvinTemperature > AtmosphericConstants.TEMPERATURE_C0 + 60)
            {
                AddInjury(new Burn(0.1f * deltaTime));
            }
        }

    }
}
