using GJ2022.Atmospherics;
using GJ2022.Atmospherics.Gasses;
using GJ2022.Components;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items.Clothing;
using GJ2022.Entities.Pawns.Health.Bodyparts.Limbs;
using GJ2022.Game.GameWorld;

namespace GJ2022.Entities.Pawns
{
    public partial class Pawn
    {

        /// <summary>
        /// Breath source counter is used for tracking the amount of breath masks a pawn
        /// has. Simpler than sending a synchronous signal every time, since when the item
        /// with the breath mask component is equipped, this is increased and when dequipped, this
        /// is decreased.
        /// </summary>
        public int BreathSourceCounter { get; set; } = 0;

        //Get wherver we are breathing from
        public Atmosphere GetBreathSource()
        {
            if (BreathSourceCounter > 0)
            {
                Atmosphere breathSource = SendSignalSynchronously(Signal.SIGNAL_PAWN_GET_INTERNAL_ATMOSPHERE) as Atmosphere;
                if (breathSource != null && breathSource.GetMoles(Oxygen.Singleton) > 0.1f)
                {
                    return breathSource;
                }
            }
            //Return atmosphere at the current location
            return World.GetTurf((int)Position[0], (int)Position[1])?.Atmosphere?.ContainedAtmosphere;
        }

        /// <summary>
        /// Check if we have an internal tank.
        /// Used for pathfinding
        /// </summary>
        public bool HasInternalTank()
        {
            if (BreathSourceCounter > 0)
            {
                Atmosphere breathSource = SendSignalSynchronously(Signal.SIGNAL_PAWN_GET_INTERNAL_ATMOSPHERE) as Atmosphere;
                if (breathSource != null && breathSource.GetMoles(Oxygen.Singleton) > 0.1f)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsPressureProtected()
        {
            BodyCoverFlags lowPressureCoveredFlags = BodyCoverFlags.NONE;
            foreach (IEquippable equippable in EquippedItems.Values)
            {
                if (equippable == null || (equippable.ProtectedHazards & PawnBehaviours.PawnHazards.HAZARD_LOW_PRESSURE) == 0)
                    continue;
                lowPressureCoveredFlags |= equippable.CoverFlags;
            }
            foreach (Limb limb in PawnBody.InsertedLimbs.Values)
            {
                //Limb doesn't exist
                if (limb == null)
                    continue;
                //Limb can handle low pressure
                if (limb.LowPressureDamage <= 0)
                    continue;
                //Check for clothing
                if (limb.IsCovered(lowPressureCoveredFlags))
                    continue;
                //Not protected
                return false;
            }
            return true;
        }

    }
}
