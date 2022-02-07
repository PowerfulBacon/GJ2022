namespace GJ2022.Entities.Pawns
{
    public enum InventorySlot
    {
        NONE = 0,
        SLOT_BACK = 1 << 0,
        SLOT_BODY = 1 << 1,
        SLOT_HEAD = 1 << 2,
        SLOT_MASK = 1 << 3,
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
                case InventorySlot.SLOT_MASK:
                    return "mask";
                default:
                    return null;
            }
        }
    }

}
