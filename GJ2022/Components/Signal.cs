using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components
{
    public enum Signal
    {
        SIGNAL_ENTITY_MOVED,
        SIGNAL_ENTITY_DESTROYED,
        //=====Block Signals=====
        //These signals are called when something has happened (mouse click) and return
        //true if something happens that should make other things not happen.
        SIGNAL_RIGHT_CLICKED,       //!Called when something is right clicked, assuming it has code to handle that () => (bool)
        //=====Request Signals=====
        //These signals indicate that an action should be performed.
        SIGNAL_ITEM_EQUIP_TO_PAWN,  //!Called when an item should be equipped to a specific pawn (Pawn) => ()
        //=====Response Signals=====
        //These signals are called after an action has been performed.
        SIGNAL_ITEM_PICKED_UP,      //!Signal called when an item has been picked up (Pawn) => ()
        SIGNAL_ITEM_EQUIPPED,       //!TODO Signal called when an item was equipped by a pawn (Pawn, InventorySlot) => ()
        SIGNAL_ITEM_UNEQUIPPED,     //!TODO Signal called when an item is unequipped by a pawn (Pawn, InventorySlot) => ()
        SIGNAL_ITEM_DROPPED,        //!Signal called hwen an item was dropped by a pawn (Pawn) => ()
        //=====Get Signals=====
        //These signals indicate that some value is wanted.
        //Gas Storage Signals
        SIGNAL_GET_ATMOSPHERE,      //! Return the atmospheric contents () => (Atmosphere)
        SIGNAL_PAWN_GET_INTERNAL_SOURCE //!TODO Returns the source of internals for a pawn () => ()
    }
}
