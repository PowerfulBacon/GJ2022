using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items;
using GJ2022.Entities.Items.Clothing;
using GJ2022.Managers.TaskManager;
using GJ2022.PawnBehaviours;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.Entities.Pawns
{
    public partial class Pawn
    {

        //Equipped items
        public Dictionary<InventorySlot, IEquippable> EquippedItems = new Dictionary<InventorySlot, IEquippable>();
        //Flags of hazards we are protected from due to our equipped items
        private PawnHazards cachedHazardProtection = PawnHazards.NONE;

        //Held items
        public Item[] heldItems = new Item[2];

        //Current hidden bodyparts
        public ClothingFlags HiddenBodypartsFlags { get; private set; } = ClothingFlags.NONE;

        /// <summary>
        /// Thread safe item equip
        /// </summary>
        public bool TryEquipItem(InventorySlot targetSlot, IEquippable item)
        {
            return ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_PAWN_EQUIPPABLES, () =>
            {
                //Check the slot
                if (EquippedItems.ContainsKey(targetSlot))
                    return false;
                EquippedItems.Add(targetSlot, item);
                item.OnEquip(this, targetSlot);
                RecalculateHazardProtection();
                AddEquipOverlay(targetSlot, item);
                //Update bodypart hiding
                ClothingFlags oldFlags = HiddenBodypartsFlags;
                HiddenBodypartsFlags |= item.ClothingFlags;
                PawnBody.UpdateLimbOverlays(Renderable, oldFlags, HiddenBodypartsFlags);
                return true;
            });
        }

        /// <summary>
        /// Add the equip overlay to the mob.
        /// Virtual class that does nothing, overriden on humans and other mobs that actually use worn overlays.
        /// </summary>
        protected virtual void AddEquipOverlay(InventorySlot targetSlot, IEquippable item) { }

        /// <summary>
        /// Recalculate what hazards we are protected from
        /// </summary>
        private void RecalculateHazardProtection()
        {
            cachedHazardProtection = PawnHazards.NONE;
            foreach (IEquippable item in EquippedItems.Values)
            {
                cachedHazardProtection |= item.ProtectedHazards;
            }
        }

        public List<Item> GetHeldItems()
        {
            List<Item> items = new List<Item>();
            for (int i = 0; i < heldItems.Length; i++)
            {
                if (heldItems[i] == null)
                    continue;
                if (heldItems[i].Destroyed || heldItems[i].Location != this)
                {
                    heldItems[i] = null;
                    continue;
                }
                items.Add(heldItems[i]);
            }
            return items;
        }

        public bool TryPickupItem(Item item)
        {
            return ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_PAWN_INVENTORY, () =>
            {
                //Destroyed items cannot be picked up
                if (item.Destroyed)
                    return false;
                //Can't pickup if the item was moved somewhere else
                if (!InReach(item))
                    return false;
                //Hands check
                int freeIndex = -1;
                for (int i = heldItems.Length - 1; i >= 0; i--)
                {
                    if (heldItems[i] == null)
                    {
                        freeIndex = i;
                        break;
                    }
                }
                //If we have no hands return false
                if (freeIndex == -1)
                    return false;
                //Pickup the item
                heldItems[freeIndex] = item;
                item.Location = this;
                return true;
            });
        }

        public bool HasFreeHands()
        {
            return ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_PAWN_INVENTORY, () =>
            {
                for (int i = heldItems.Length - 1; i >= 0; i--)
                {
                    if (heldItems[i] == null)
                        return true;
                    if (heldItems[i].Destroyed)
                    {
                        heldItems[i] = null;
                        return true;
                    }
                }
                return false;
            });
        }

        public bool IsHoldingItems()
        {
            return ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_PAWN_INVENTORY, () =>
            {
                for (int i = heldItems.Length - 1; i >= 0; i--)
                {
                    if (heldItems[i] != null)
                    {
                        if (!heldItems[i].Destroyed)
                            return true;
                        heldItems[i] = null;
                    }
                }
                return false;
            });
        }

        public bool DropFirstItem(Vector<float> dropLocation)
        {
            return ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_PAWN_INVENTORY, () =>
            {
                for (int i = heldItems.Length - 1; i >= 0; i--)
                {
                    if (heldItems[i] == null)
                        continue;
                    if (heldItems[i].Destroyed)
                    {
                        heldItems[i] = null;
                        continue;
                    }
                    //Drop the item out of ourselves
                    heldItems[i].Location = null;
                    heldItems[i].Position = dropLocation.Copy();
                    heldItems[i] = null;
                    return true;
                }
                return false;
            });
        }

        public void DropHeldItems(Vector<float> dropLocation)
        {
            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_PAWN_INVENTORY, () =>
            {
                for (int i = heldItems.Length - 1; i >= 0; i--)
                {
                    if (heldItems[i] == null)
                        continue;
                    if (heldItems[i].Destroyed)
                    {
                        heldItems[i] = null;
                        continue;
                    }
                    //Drop the item out of ourselves
                    heldItems[i].Position = dropLocation.Copy();
                    heldItems[i].Location = null;
                    heldItems[i] = null;
                }
                return true;
            });
        }

    }
}
