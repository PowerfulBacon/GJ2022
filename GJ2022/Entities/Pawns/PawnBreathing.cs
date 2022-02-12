using GJ2022.Atmospherics;
using GJ2022.Atmospherics.Gasses;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items.Clothing;
using GJ2022.Entities.Pawns.Health.Bodyparts.Limbs;
using GJ2022.Game.GameWorld;

namespace GJ2022.Entities.Pawns
{
    public partial class Pawn
    {

        //Get wherver we are breathing from
        public Atmosphere GetBreathSource()
        {
            //Check if we can breathe from internals
            IBreathMask breathMask = (EquippedItems.ContainsKey(InventorySlot.SLOT_MASK) ? EquippedItems[InventorySlot.SLOT_MASK] : null) as IBreathMask;
            if (breathMask != null)
            {
                Atmosphere breathSource = breathMask.GetBreathSource();
                if (breathSource != null && breathSource.GetMoles(Oxygen.Singleton) > 0.1f)
                    return breathSource;
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
            IBreathMask breathMask = (EquippedItems.ContainsKey(InventorySlot.SLOT_MASK) ? EquippedItems[InventorySlot.SLOT_MASK] : null) as IBreathMask;
            if (breathMask != null)
            {
                Atmosphere breathSource = breathMask.GetBreathSource();
                if (breathSource != null && breathSource.GetMoles(Oxygen.Singleton) > 0.1f)
                    return true;
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
