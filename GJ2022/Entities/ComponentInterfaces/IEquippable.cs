﻿using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours;

namespace GJ2022.Entities.ComponentInterfaces
{
    public interface IEquippable
    {

        //The slot flags it can be put in.
        InventorySlot Slots { get; }

        //Hazards this item protects from when equipped
        PawnHazards ProtectedHazards { get; }

        //The texture when equpped, appended with _slot
        string EquipTexture { get; }

        //When the item is equipped
        void OnEquip(Pawn pawn, InventorySlot slot);

        //When the item is unequipped
        void OnUnequip(Pawn pawn, InventorySlot slot);

    }
}
