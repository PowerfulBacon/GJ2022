using GJ2022.Atmospherics;
using GJ2022.Entities.Pawns.Health.Bodyparts;
using GJ2022.Entities.Pawns.Health.Bodyparts.Limbs;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs;
using GJ2022.Entities.Pawns.Health.Injuries.Instances.Generic;
using GJ2022.Entities.Turfs;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using System;
using System.Collections.Generic;
using System.Linq;

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
            Turf location = World.GetTurf((int)Parent.Position[0], (int)Parent.Position[1]);
            float pressure = location?.Atmosphere?.ContainedAtmosphere.KiloPascalPressure ?? 0;
            for (int i = InsertedLimbs.Count - 1; i >= 0; i--)
            {
                Limb limb = InsertedLimbs.Values.ElementAt(i);
                if (limb == null || (limb.limbFlags & LimbFlags.LIMB_DESTROYED) != 0)
                    continue;
                if (limb.LowPressureDamage > pressure)
                {
                    //Apply low pressure damage proportionally
                    //4 damage per second at 0 pressure
                    //1 damage per second at 20 pressure
                    limb.AddInjury(new Crush(deltaTime * GetLowPressureDamageMultiplier(4, limb.LowPressureDamage, pressure)));
                }
                else if (limb.HighPressureDamage < pressure)
                {
                    //Apply high pressure damage
                    limb.AddInjury(new Crush(deltaTime * (float)Math.Sqrt(pressure - limb.HighPressureDamage)));
                }
            }
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

        /// <summary>
        /// Starts at 0 and rises as you approach pressure = 0
        /// MultiplierMax = the multiplier for being at 0 pressure
        /// PressureLimit = The point at which the pressure multiplier is 1
        /// Pressure = Current pressure
        /// 
        /// x = pressure
        /// y = multiplier
        /// 
        /// 1/x curve that passes through (pressureLimit, 1) and (0, multiplierMax)
        /// 
        /// a = pressureLimit
        /// b = multiplierMax
        /// 
        /// y=\frac{a}{x+c}+d
        /// d=\frac{a+ab-\sqrt{\left(-a-ab\right)\left(-a-ab\right)-4a^{2}}}{2a}
        /// c=\frac{da}{1-d}
        /// </summary>
        private float GetLowPressureDamageMultiplier(float multiplierMax, float pressureLimit, float pressure)
        {
            //TODO: sqrt is too laggy.
            float ab = pressureLimit * multiplierMax;
            float d = (pressureLimit + pressureLimit * multiplierMax - (float)Math.Sqrt((-pressureLimit - ab) * (-pressureLimit - ab) - 4 * (pressureLimit * pressureLimit))) / (2 * pressureLimit);
            float c = d * multiplierMax / (1 - d);
            return multiplierMax / (pressure + c) + d;
        }

    }
}
