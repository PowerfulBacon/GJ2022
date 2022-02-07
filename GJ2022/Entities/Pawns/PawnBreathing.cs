using GJ2022.Atmospherics;
using GJ2022.Atmospherics.Gasses;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
