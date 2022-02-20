using GJ2022.Entities;
using GJ2022.Entities.Items;
using GJ2022.Entities.Items.Clothing;
using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours;
using GJ2022.PawnBehaviours.PawnActions;
using GJ2022.Subsystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Items
{
    public class Component_Equippable : Component
    {

        /// <summary>
        /// The overlay texture to apply to pawns when equipped.
        /// </summary>
        public string EquipTexture { get; private set; }

        /// <summary>
        /// Should we append the equipped slot to the item name?
        /// </summary>
        public bool AppendSlotToIconState { get; private set; } = false;

        public InventorySlot Slots => InventorySlot.SLOT_MASK;

        public PawnHazards ProtectedHazards => PawnHazards.NONE;

        public ClothingFlags ClothingFlags => ClothingFlags.NONE;

        public BodyCoverFlags CoverFlags => BodyCoverFlags.COVER_MOUTH;

        public override void OnComponentAdd()
        {
            Parent.RegisterSignal(Signal.SIGNAL_ITEM_EQUIPPED, -1, OnEquip);
            Parent.RegisterSignal(Signal.SIGNAL_ITEM_UNEQUIPPED, -1, OnUnequip);
            Parent.RegisterSignal(Signal.SIGNAL_RIGHT_CLICKED, 5, OnRightClicked);
            Parent.RegisterSignal(Signal.SIGNAL_ITEM_EQUIP_TO_PAWN, -1, TryEquipTo);
        }

        public override void OnComponentRemove()
        {
            Parent.UnregisterSignal(Signal.SIGNAL_ITEM_EQUIPPED, OnEquip);
            Parent.UnregisterSignal(Signal.SIGNAL_ITEM_UNEQUIPPED, OnUnequip);
            Parent.UnregisterSignal(Signal.SIGNAL_RIGHT_CLICKED, OnRightClicked);
            Parent.UnregisterSignal(Signal.SIGNAL_ITEM_EQUIP_TO_PAWN, TryEquipTo);
        }

        private object TryEquipTo(object parent, params object[] arguments)
        {
            Pawn targetPawn = arguments[0] as Pawn;
            //Convert slot flags to an actual slot
            InventorySlot wantedSlot;
            if ((Slots & InventorySlot.SLOT_BACK) != 0)
                wantedSlot = InventorySlot.SLOT_BACK;
            else if ((Slots & InventorySlot.SLOT_BODY) != 0)
                wantedSlot = InventorySlot.SLOT_BODY;
            else if ((Slots & InventorySlot.SLOT_HEAD) != 0)
                wantedSlot = InventorySlot.SLOT_HEAD;
            else if ((Slots & InventorySlot.SLOT_MASK) != 0)
                wantedSlot = InventorySlot.SLOT_MASK;
            else
                return null;
            targetPawn.TryEquipItem(wantedSlot, this);
            return null;
        }

        private object OnRightClicked(object parent, params object[] arguments)
        {
            if (PawnControllerSystem.Singleton.SelectedPawn == null)
                return false;
            PawnControllerSystem.Singleton.SelectedPawn.behaviourController?.PawnActionIntercept(new EquipItem(Parent as Item));
            return true;
        }

        /// <summary>
        /// Delegate executed when the parent item was equipped by a pawn
        /// </summary>
        private object OnEquip(object parent, params object[] arguments)
        {
            Entity parentEntity = (Entity)parent;
            Entity equippedEntity = (Entity)arguments[0];
            parentEntity.Location = equippedEntity;
            return null;
        }

        /// <summary>
        /// Delegate executed when the parent item was unequipped
        /// </summary>
        private object OnUnequip(object parent, params object[] arguments)
        {
            Entity parentEntity = (Entity)parent;
            Entity equippedEntity = (Entity)arguments[0];
            parentEntity.Location = null;
            parentEntity.Position = equippedEntity.Position;
            return null;
        }

        public override void SetProperty(string name, object property)
        {
            //TODO
            return;
        }

    }
}
