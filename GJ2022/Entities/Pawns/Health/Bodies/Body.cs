using GJ2022.Atmospherics;
using GJ2022.Components.Items;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items.Clothing;
using GJ2022.Entities.Pawns.Health.Bodyparts;
using GJ2022.Entities.Pawns.Health.Bodyparts.Limbs;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs;
using GJ2022.Entities.Pawns.Health.Injuries;
using GJ2022.Entities.Pawns.Health.Injuries.Instances.Generic;
using GJ2022.Entities.Turfs;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GJ2022.Entities.Pawns.Health.Bodies
{
    public abstract class Body : IProcessable
    {

        private static Random random = new Random();

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

        //The value used to calculate the amount of pain a pawn can take before it goes unconcious.
        //The pain limit is calculated by taking the sum of the health of all limbs and multiplying that by this multiplier.
        public virtual float PainCritMultiplier => 0.5f;

        //Pain Threashold
        //If not defined, will be created during init
        public virtual float PainCritLimit { get; internal set; }

        //Pain
        public float Pain { get; internal set; } = 0;

        //Stats
        public float Conciousness { get; internal set; } = 0;
        public float Movement { get; internal set; } = 0;
        public float Manipulation { get; internal set; } = 0;
        public float Vision { get; internal set; } = 0;
        public float Hearing { get; internal set; } = 0;

        //Does this body use gender?
        //Robots and constructs shouldn't have a gender, while dogs and people should
        public abstract bool HasGender { get; }

        //Gender of the mob
        public Genders Gender { get; }

        //Does the body support limb overlays?
        public virtual bool SupportsLimbOverlays { get; } = false;

        //Does the body require blood?
        public virtual bool RequiresBlood { get; } = true;

        //The blood efficiency
        //Based on the amount of blood in the body. As a pawn bleeds, the blood efficiency
        //will decrease until there is not enough blood to carry enough oxygen to the brain.
        public float BloodEfficiency { get; private set; } = 1.0f;

        //The maximum volume of blood in this body
        //In ml
        public abstract float MaximumBloodVolume { get; }

        //Amount of blood currently in the body
        public float BloodVolume { get; private set; }

        //The proportion of blood that is considered safe, if blood volume > MaximumBloodVolume * DefaultBloodProportion, efficiency will be 1.
        protected abstract float DefaultBloodProportion { get; }

        //Rate at which bleeding naturally fixes itself in ml per second per second
        //y=\frac{1}{2}ax^{2}+\frac{1}{2}x where BleedFixRate = a
        public abstract float BleedFixRate { get; }

        //Rate at which we are currently bleeding, in ml per second.
        public float BleedRate { get; private set; } = 0;

        public bool Destroyed => false;

        //Temp
        public bool NoDamage { get; set; } = true;

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
            //Assign gender
            if (HasGender)
            {
                if (random.NextDouble() > 0.5f)
                    Gender = Genders.MALE;
                else
                    Gender = Genders.FEMALE;
            }
            else
            {
                Gender = Genders.AGENDER;
            }
            //Set the blood
            if (RequiresBlood)
            {
                BloodVolume = MaximumBloodVolume * DefaultBloodProportion;
            }
        }

        public void SetupBody(Pawn parent)
        {
            Parent = parent;
            CreateDefaultBodyparts();
            //Calculate pain crit threshold
            if (PainCritLimit == default)
            {
                foreach (Bodypart part in InsertedLimbs.Values)
                {
                    PainCritLimit += part?.MaxHealth ?? 0;
                }
                PainCritLimit *= PainCritMultiplier;
            }
        }

        protected abstract void CreateDefaultBodyparts();

        protected void SetupAndInsertLimb(Limb limb, BodySlots slot)
        {
            limb.SetupOrgans(Parent, this);
            limb.Insert(this, slot);
        }

        public void Process(float deltaTime)
        {
            //Process bleeding
            ProcessBleeding(deltaTime);
            //Process pressure damage (TODO: If not protected via pressure resistant clothing)
            Turf location = World.GetTurf((int)Parent.Position.X, (int)Parent.Position.Y);
            float pressure = location?.Atmosphere?.ContainedAtmosphere.KiloPascalPressure ?? 0;
            for (int i = InsertedLimbs.Count - 1; i >= 0; i--)
            {
                Limb limb = InsertedLimbs.Values.ElementAt(i);
                //Check if the limb is destroyed
                if (limb == null || (limb.limbFlags & LimbFlags.LIMB_DESTROYED) != 0)
                    continue;
                //If not do stuff
                if (limb.LowPressureDamage > pressure)
                {
                    //Check if the limb is covered
                    bool limbProtected = false;
                    foreach (Component_Equippable equippable in Parent.EquippedItems.Values)
                    {
                        if ((equippable.ProtectedHazards & PawnBehaviours.PawnHazards.HAZARD_LOW_PRESSURE) != 0 && limb.IsCovered(equippable.CoverFlags))
                        {
                            limbProtected = true;
                            break;
                        }
                    }
                    //Apply low pressure damage proportionally
                    //4 damage per second at 0 pressure
                    //1 damage per second at 20 pressure
                    if (!limbProtected)
                        limb.AddInjury(new Crush(deltaTime * GetLowPressureDamageMultiplier(4, limb.LowPressureDamage, pressure)));
                }
                else if (limb.HighPressureDamage < pressure)
                {
                    //Check if the limb is covered
                    bool limbProtected = false;
                    foreach (Component_Equippable equippable in Parent.EquippedItems.Values)
                    {
                        if ((equippable.ProtectedHazards & PawnBehaviours.PawnHazards.HAZARD_HIGH_PRESSURE) != 0 && limb.IsCovered(equippable.CoverFlags))
                        {
                            limbProtected = true;
                            break;
                        }
                    }
                    if (!limbProtected)
                        //Apply high pressure damage
                        //TODO
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
        /// Adjusts the pain by the provided amount and crits the pawn if they have too much pain
        /// </summary>
        /// <param name="adjustAmount"></param>
        public void AdjustPain(float adjustAmount)
        {
            Pain += adjustAmount;
            if (Pain > PainCritLimit)
                Parent.EnterCrit();
            else
                Parent.ExitCrit();
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

        public string GetGenderText()
        {
            switch (Gender)
            {
                case Genders.AGENDER:
                    return "agender";
                case Genders.FEMALE:
                    return "female";
                case Genders.MALE:
                    return "male";
            }
            return "agender";
        }

        public string GetRandomHaircut()
        {
            switch (Gender)
            {
                case Genders.MALE:
                    return ListPicker.Pick(maleHaircuts);
                case Genders.FEMALE:
                    return ListPicker.Pick(femaleHaircuts);
                default:
                    return ListPicker.Pick(new string[] { ListPicker.Pick(maleHaircuts), ListPicker.Pick(femaleHaircuts) });
            }
        }

        /// <summary>
        /// Update limb overlays to account for new cover flags
        /// </summary>
        public void UpdateLimbOverlays(Renderable renderable, ClothingFlags oldFlags, ClothingFlags newFlags)
        {
            if (!SupportsLimbOverlays)
                return;
            foreach (Limb limb in InsertedLimbs.Values)
            {
                //Nothing inserted in this slot.
                if (limb == null)
                    continue;
                //Update the limb
                limb.UpdateCoveredOverlays(renderable, oldFlags, newFlags);
                //Update limbs children
                foreach (Organ organ in limb.containedOrgans)
                {
                    organ.UpdateCoveredOverlays(renderable, oldFlags, newFlags);
                }
            }
        }

        /// <summary>
        /// Process blood loss as well as bleed healing
        /// </summary>
        private void ProcessBleeding(float deltaTime)
        {
            //Decrease blood
            BloodVolume = Math.Max(BloodVolume - BleedRate * deltaTime, 0.0f);
            //Update bleed efficiency
            BloodEfficiency = Math.Min(BloodVolume / (DefaultBloodProportion * MaximumBloodVolume), 1.0f);
            //Update the bleed rate to account for bleeding healing over time
            BleedRate = Math.Max(BleedRate - BleedFixRate * deltaTime, 0.0f);
        }

        /// <summary>
        /// Change the bleeding rate by the specified amount.
        /// </summary>
        public void AdjustBleed(float bleedRateDelta)
        {
            BleedRate += bleedRateDelta;
        }

        /// <summary>
        /// Applies an injury to a random bodypart.
        /// </summary>
        /// <param name="injury"></param>
        public bool ApplyDamageRandomly(Injury injury)
        {
            Limb selectedLimb = GetRandomWorkingLimb();
            if (selectedLimb == null)
                return false;
            selectedLimb.AddInjury(injury);
            return true;
        }

        private Limb GetRandomWorkingLimb()
        {
            List<Limb> limbs = new List<Limb>();
            foreach (Limb limb in InsertedLimbs.Values)
                if (limb != null && limb.Health > 0)
                    limbs.Add(limb);
            if (limbs.Count == 0)
                return null;
            return ListPicker.Pick(limbs);
        }

        public bool Destroy() => false;

        //TODO: Put these in a txt data file or at least in its own .cs file

        //Sort haircuts into male and female haircuts
        //Generally this is done by length for easier identification even
        //if most haircuts are for either genders
        private string[] maleHaircuts = new string[] {
            "hair_a",
            "hair_c",
            "hair_d",
            "hair_e",
            "hair_f",
            "hair_bedhead",
            "hair_jensen",
            "hair_skinhead",
            "hair_halfbang",
            "hair_kusanagi",
            "hair_ponytail5",
            "hair_halfbang2",
            "hair_bedheadv2",
            "hair_curls",
            "hair_bedheadv3",
            "hair_spikey",
            "hair_bigafro",
            "hair_afro",
            "hair_gelled",
            "hair_sargeant",
            "hair_afro2",
            "hair_bobcurl",
            "hair_bobcut",
            "hair_bowlcut",
            "hair_buzzcut",
            "hair_combover",
            "hair_crewcut",
            "hair_devilock",
            "hair_emo",
            "hair_hitop",
            "hair_parted",
            "hair_pompadour",
            "hair_quiff",
            "hair_part",
            "hair_protagonist",
            "hair_messy",
            "hair_shorthair2",
            "hair_keanu",
            "hair_swept",
            "hair_business2",
            "hair_hedgehog",
            "hair_business",
            "hair_shorthair3",
            "hair_bob",
            "hair_bigflattop",   //wtf is this haircut
            "hair_bigpompadour",
            "hair_spiky",
            "hair_spiky2",
            "hair_business3",
            "hair_business4",
            "hair_bob2",
            "hair_swept2",
            "hair_reversemohawkold",
            "hair_megaeyebrows",
            "hair_boddicker",
            "hair_rosa",
            "hair_emofringe",
            "hair_unshaven_mohawk",
            "hair_manbun",
            "hair_veryshortovereyealternate",
            "hair_shorthime",
            "hair_thinningfront",
            "hair_cia",
            "hair_mulder",
            "hair_joestar",
            "hair_shortbangs",
            "hair_undercutleft",
            "hair_oxton",
            "hair_fringetail",
            "hair_undercutright",
            "hair_cornrowbun",
            "hair_cornrowtail",
            "hair_cornrowbraid",
            "hair_shavedmohawk",
            "hair_ronin",
            "hair_bowlcut2",
            "hair_thinningrear",
            "hair_thinning",
            "hair_father",
            "hair_emo2",
            "hair_lowfade",
            "hair_medfade",
            "hair_highfade",
            "hair_baldfade",
            "hair_nofade",
            "hair_trimflat",
            "hair_shaved",
            "hair_trimmed",
            "hair_tightbun",
            "hair_cornrows",
            "hair_cornrows2",
            "hair_dandypompadour",
            "hair_coffeehouse",
            "hair_shavedpart",
            "hair_undercut",
            "hair_toriyama",
            "hair_doublespikes",
            "hair_shortafro",
            "hair_ebgel",
            "hair_curtains",
            "hair_hugepompadour",
            "hair_reversemohawk",
            "hair_mullet",
            "hair_moustache",
            "hair_spamton",
            "hair_pride"
        };
        private string[] femaleHaircuts = new string[] {
            "hair_b",
            "hair_dreads",
            "hair_vlong",
            "hair_ponytail",
            "hair_ponytail2",
            "hair_longfringe",
            "hair_himecut",
            "hair_himecut2",
            "hair_himeup",
            "hair_vlongfringe",
            "hair_longest",
            "hair_longest2",
            "hair_braid",
            "hair_braid2",
            "hair_kagami",
            "hair_beehive",
            "hair_feather",
            "hair_odango",
            "hair_ombre",
            "hair_updo",
            "hair_lbangs",
            "hair_braided",
            "hair_bun",
            "hair_sidetail",
            "hair_sidetail2",
            "hair_braidfront",
            "hair_antenna",
            "hair_pigtails",
            "hair_sidetail3",
            "hair_hbraid",
            "hair_gentle",
            "hair_oneshoulder",
            "hair_tressshoulder",
            "hair_braidtail",
            "hair_ponytail3",
            "hair_longovereye",
            "hair_bunhead2",
            "hair_ponytail4",
            "hair_long",
            "hair_long2",
            "hair_drillhair",
            "hair_pigtails2",
            "hair_pixie",
            "hair_sidetail4",
            "hair_beehivev2",
            "hair_largebun",
            "hair_shortbraid",
            "hair_longemo",
            "hair_shortovereye",
            "hair_longsidepart",
            "hair_longstraightponytail",
            "hair_highponytail",
            "hair_80s",
            "hair_floorlength_bedhead",
            "hair_country",
            "hair_ponytail7",
            "hair_ponytail6",
            "hair_long_bedhead",
            "hair_stail",
            "hair_nitori",
            "hair_halfshaved",
            "hair_bun3",
            "hair_wisp",
            "hair_spikyponytail",
            "hair_topknot",
            "hair_jade",
            "hair_flair",
            "hair_doublebun",
            "hair_twintail",
            "hair_volaju",
            "hair_poofy",
            "hair_drillruru",
            "hair_modern",
            "hair_unkept",
            "hair_shorthairg",
            "hair_bob4",
            "hair_parted2",
            "hair_tightponytail"
        };

    }
}
