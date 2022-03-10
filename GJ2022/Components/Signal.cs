using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components
{
    public enum Signal
    {
        SIGNAL_ENTITY_DESTROYED,
        //=====Block Signals=====
        //These signals are called when something has happened (mouse click) and return
        //true if something happens that should make other things not happen.
        SIGNAL_RIGHT_CLICKED,       //!Called when something is right clicked, assuming it has code to handle that () => (bool)
        //=====Request Signals=====
        //These signals indicate that an action should be performed.
        SIGNAL_ITEM_EQUIP_TO_PAWN,  //! Called when an item should be equipped to a specific pawn (Pawn) => ()
        SIGNAL_ITEM_GIVE_POWER,     //! Give power to something (int: amount) => (float: remaining)
        SIGNAL_ITEM_TAKE_POWER,     //! Take power from something if available (float: amountWanted) => (float: amountSupplied)
        SIGNAL_ENTITY_MINE,         //! Mine an entity (Pawn: miner) => ()
        SIGNAL_SET_STACK_SIZE,      //! Set the stack size of an entity (int: amount) => ()
        //=====Response Signals=====
        //These signals are called after an action has been performed.
        SIGNAL_ITEM_PICKED_UP,      //!Signal called when an item has been picked up (Pawn) => ()
        SIGNAL_ITEM_EQUIPPED,       //!TODO Signal called when an item was equipped by a pawn (Pawn, InventorySlot) => ()
        SIGNAL_ITEM_UNEQUIPPED,     //!TODO Signal called when an item is unequipped by a pawn (Pawn, InventorySlot) => ()
        SIGNAL_ITEM_DROPPED,        //!Signal called when an item was dropped by a pawn (Pawn) => ()
        SIGNAL_ENTITY_MOVED,        //!Signal called when an entity changes position (Vector<float> position, Vector<flota> oldPosition) => ()
        SIGNAL_ENTITY_LOCATION,     //!Signal called when an entity changes loc (Entity loc, Entity oldLoc) => ()
        SIGNAL_AREA_CONTENTS_ADDED, //!Signal called when an item is added to the contents of an area (Item item) => ()
        SIGNAL_AREA_CONTENTS_REMOVED,   //!Signal called when an item is removed from the contents of an area (Item item) => ()
        //=====Get Signals=====
        //These signals indicate that some value is wanted.
        //Gas Storage Signals
        SIGNAL_GET_ATMOSPHERE,      //! Return the atmospheric contents () => (Atmosphere)
        SIGNAL_PAWN_GET_INTERNAL_ATMOSPHERE,    //!TODO Returns the atmosphere source of internals for a pawn () => ()
        SIGNAL_GET_STORED_POWER,    //! Return the amount of power stored in this object () => (float: storedPower)
        SIGNAL_GET_COUNT,           //! Return the amount of items represented by this entity () => (int: count)
        SIGNAL_GET_POWER_DEMAND,    //! Get the power demand of this signal (float: deltaTime) => (float: demand)
    }
}
