using GJ2022.Entities.Pawns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Items
{
    public class Component_BreathSource : Component
    {

        public override void OnComponentAdd()
        {
            Parent.RegisterSignal(Signal.SIGNAL_ITEM_EQUIPPED, -1, OnEquip);
            Parent.RegisterSignal(Signal.SIGNAL_ITEM_UNEQUIPPED, -1, OnUnequip);
        }

        public override void OnComponentRemove()
        {
            Parent.UnregisterSignal(Signal.SIGNAL_ITEM_EQUIPPED, OnEquip);
            Parent.UnregisterSignal(Signal.SIGNAL_ITEM_UNEQUIPPED, OnUnequip);
        }

        private object OnEquip(object parent, params object[] arguments)
        {
            Pawn equippedEntity = (Pawn)arguments[0];
            equippedEntity.BreathSourceCounter++;
            return null;
        }

        /// <summary>
        /// Delegate executed when the parent item was unequipped
        /// </summary>
        private object OnUnequip(object parent, params object[] arguments)
        {
            Pawn equippedEntity = (Pawn)arguments[0];
            equippedEntity.BreathSourceCounter--;
            return null;
        }

        public override void SetProperty(string name, object property)
        {
            throw new NotImplementedException($"BreathSource component does not implement {name}.");
        }

    }
}
