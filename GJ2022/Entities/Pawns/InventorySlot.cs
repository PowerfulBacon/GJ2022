using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns
{
    public enum InventorySlot
    {
        NONE = 0,
        SLOT_BACK = 1 << 0,
        SLOT_BODY = 1 << 1,
        SLOT_HEAD = 1 << 2,
    }

    public static class InventoryHelper
    {
        public static string GetSlotAppend(InventorySlot slot)
        {
            switch (slot)
            {
                case InventorySlot.SLOT_BACK:
                    return "back";
                case InventorySlot.SLOT_BODY:
                    return "body";
                case InventorySlot.SLOT_HEAD:
                    return "head";
            }
            return null;
        }
    }

}
