using GJ2022.Areas;
using GJ2022.Entities.Items;
using GJ2022.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Managers
{
    public static class StockpileManager
    {

        //List of items in the stockpile
        //First key = The item type
        //Second key = the stockpile area the items are stored in
        //Value = A list of items with the type and stockpile area specified
        public static Dictionary<Type, List<Item>> StockpileItems { get; } = new Dictionary<Type, List<Item>>();

        /// <summary>
        /// Returns the reference to an item in the stockpile.
        /// Returns null if the requested type is not in the stockpile.
        /// </summary>
        public static Item LocateItemInStockpile(Type wantedType)
        {
            if (!StockpileItems.ContainsKey(wantedType))
                return null;
            //Choose a stockpile to go to
            List<Item> targetItems = ListPicker.Pick(StockpileItems.Values);
            //Pick an item in that stockpile.
            return ListPicker.Pick(targetItems);
        }

        public static void AddItem(Item item)
        {
            StockpileItems.Add(item.GetType(), new List<Item>() { item });
        }

        public static void RemoveItem(Item item)
        {

        }

    }
}
