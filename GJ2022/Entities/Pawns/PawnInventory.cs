using GJ2022.Components;
using GJ2022.Components.Items;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items;
using GJ2022.Entities.Items.Clothing;
using GJ2022.Managers.TaskManager;
using GJ2022.PawnBehaviours;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;

namespace GJ2022.Entities.Pawns
{
    public partial class Pawn
    {

        //Equipped items
        public Dictionary<InventorySlot, Component_Equippable> EquippedItems = new Dictionary<InventorySlot, Component_Equippable>();
        //Flags of hazards we are protected from due to our equipped items
        private PawnHazards cachedHazardProtection = PawnHazards.NONE;

        //Held items
        public Item[] heldItems = new Item[2];

        //Current hidden bodyparts
        public ClothingFlags HiddenBodypartsFlags { get; private set; } = ClothingFlags.NONE;

        public bool TryEquipItem(InventorySlot targetSlot, Component_Equippable equippable)
        {
            lock (EquippedItems)
            {
                //Check the slot
                if (EquippedItems.ContainsKey(targetSlot))
                    return false;
                EquippedItems.Add(targetSlot, equippable);
                equippable.Parent.SendSignal(Signal.SIGNAL_ITEM_EQUIPPED, this, targetSlot);
                RecalculateHazardProtection();
                AddEquipOverlay(targetSlot, equippable);
                //Update bodypart hiding
                ClothingFlags oldFlags = HiddenBodypartsFlags;
                HiddenBodypartsFlags |= equippable.ClothingFlags;
                PawnBody.UpdateLimbOverlays(Renderable, oldFlags, HiddenBodypartsFlags);
                return true;
            }
        }

        /// <summary>
        /// Thread safe item equip
        /// </summary>
        [Obsolete]
        public bool TryEquipItem(InventorySlot targetSlot, IEquippable item)
        {
            lock (EquippedItems)
            {
                //Check the slot
                if (EquippedItems.ContainsKey(targetSlot))
                    return false;
                //EquippedItems.Add(targetSlot, item);
                //item.SendSignal(Signal.SIGNAL_ITEM_EQUIPPED, this, targetSlot);
                item.OnEquip(this, targetSlot); //TODO: Remove
                RecalculateHazardProtection();
                AddEquipOverlay(targetSlot, item);
                //Update bodypart hiding
                ClothingFlags oldFlags = HiddenBodypartsFlags;
                HiddenBodypartsFlags |= item.ClothingFlags;
                PawnBody.UpdateLimbOverlays(Renderable, oldFlags, HiddenBodypartsFlags);
                return true;
            }
        }

        /// <summary>
        /// Add the equip overlay to the mob.
        /// Virtual class that does nothing, overriden on humans and other mobs that actually use worn overlays.
        /// </summary>
        [Obsolete]
        protected virtual void AddEquipOverlay(InventorySlot targetSlot, IEquippable item) { }
        protected virtual void AddEquipOverlay(InventorySlot targetSlot, Component_Equippable equippable) { }

        /// <summary>
        /// Recalculate what hazards we are protected from
        /// </summary>
        private void RecalculateHazardProtection()
        {
            cachedHazardProtection = PawnHazards.NONE;
            foreach (Component_Equippable equippedItemComponent in EquippedItems.Values)
            {
                cachedHazardProtection |= equippedItemComponent.ProtectedHazards;
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
                //Send the pickup signal
                item.SendSignal(Signal.SIGNAL_ITEM_PICKED_UP, this);
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
                    //Send drop signal
                    heldItems[i].SendSignal(Signal.SIGNAL_ITEM_DROPPED, this);
                    //Stop referencing
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
                    //Send drop signal
                    heldItems[i].SendSignal(Signal.SIGNAL_ITEM_DROPPED, this);
                    //Stop referencing
                    heldItems[i] = null;
                }
                return true;
            });
        }

    }
}
