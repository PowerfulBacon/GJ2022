using GJ2022.Atmospherics;
using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Turfs;
using GJ2022.Game.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                ProcessLungDamage();
                return;
            }
            //Equalize gasses in lungs with the gasses in the atmosphere.
            Body.internalAtmosphere.Equalize(atmosphere);
            ProcessLungDamage();
        }

        private void ProcessLungDamage()
        {
            //TODO
            //If the temperature is too extreme, take damage
        }

    }
}
