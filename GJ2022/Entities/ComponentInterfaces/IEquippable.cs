using GJ2022.Components;
using GJ2022.Entities.Items.Clothing;
using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours;
using System;

namespace GJ2022.Entities.ComponentInterfaces
{
    [Obsolete]
    public interface IEquippable : IComponentHandler
    {

        bool AppendSlotToIconState { get; }

        //The slot flags it can be put in.
        InventorySlot Slots { get; }

        //Hazards this item protects from when equipped
        PawnHazards ProtectedHazards { get; }

        //The texture when equpped, appended with _slot
        string EquipTexture { get; }

        //Clothing flags
        //Flags for hiding facial features when worn
        ClothingFlags ClothingFlags { get; }

        //Cover flags
        //Identifies which body parts this covers (and where the hazard protection si applied to)
        BodyCoverFlags CoverFlags { get; }

        //When the item is equipped
        [Obsolete]
        void OnEquip(Pawn pawn, InventorySlot slot);

        //When the item is unequipped
        [Obsolete]
        void OnUnequip(Pawn pawn, InventorySlot slot);

    }
}
