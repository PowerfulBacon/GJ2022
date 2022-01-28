using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.ComponentInterfaces
{
    public interface IEquippable
    {

        //The slot flags it can be put in.
        InventorySlot Slots { get; }

        //Hazards this item protects from when equipped
        PawnHazards ProtectedHazards { get; }

        //When the item is equipped
        void OnEquip(Pawn pawn, InventorySlot slot);

        //When the item is unequipped
        void OnUnequip(Pawn pawn, InventorySlot slot);

    }
}
