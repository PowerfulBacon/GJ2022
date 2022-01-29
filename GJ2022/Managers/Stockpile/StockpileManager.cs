using GJ2022.Areas;
using GJ2022.Entities.Items;
using GJ2022.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GJ2022.Managers.Stockpile
{
    public static class StockpileManager
    {

        //List of items in the stockpile
        //First key = The item type
        //Second key = the stockpile area the items are stored in
        //Value = A list of items with the type and stockpile area specified
        public static Dictionary<Type, List<Item>> StockpileItems { get; } = new Dictionary<Type, List<Item>>();

        public static HashSet<StockpileArea> StockpileAreas { get; } = new HashSet<StockpileArea>();

        public static void CountItems(Type itemType)
        {
            ThreadSafeTaskManager.ExecuteThreadSafeActionUnblocking(ThreadSafeTaskManager.TASK_STOCKPILE_MANAGER, () =>
            {
                if (StockpileItems.ContainsKey(itemType))
                {
                    int count = 0;
                    foreach (Item item in StockpileItems[itemType])
                    {
                        count += item.Count();
                    }
                    StockpileUserInterfaceManager.UpdateUserInterface(StockpileItems[itemType].First(), itemType, count);
                }
                else
                {

                    StockpileUserInterfaceManager.UpdateUserInterface(null, itemType, 0);
                }
                return true;
            });
        }

        /// <summary>
        /// Returns the reference to an item in the stockpile.
        /// Returns null if the requested type is not in the stockpile.
        /// </summary>
        public static Item LocateItemInStockpile(Type wantedType)
        {
            if (!StockpileItems.ContainsKey(wantedType))
                return null;
            //Choose a stockpile to go to
            List<Item> targetItems = StockpileItems[wantedType];
            //Pick an item in that stockpile.
            return ListPicker.Pick(targetItems);
        }

        public static void AddStockpileArea(StockpileArea area)
        {
            //Add the stockpile area
            StockpileAreas.Add(area);
            //Add the stockpile area's items
            area.RegisterItemsInStockpile();
        }

        public static void AddItem(Item item)
        {
            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_STOCKPILE_MANAGER, () =>
            {
                if (StockpileItems.ContainsKey(item.GetType()))
                    StockpileItems[item.GetType()].Add(item);
                else
                    StockpileItems.Add(item.GetType(), new List<Item>() { item });
                return true;
            });
            CountItems(item.GetType());
        }

        public static void RemoveItem(Item item)
        {
            ThreadSafeTaskManager.ExecuteThreadSafeAction(ThreadSafeTaskManager.TASK_STOCKPILE_MANAGER, () =>
            {
                if (!StockpileItems.ContainsKey(item.GetType()))
                    return false;
                StockpileItems[item.GetType()].Remove(item);
                if (StockpileItems[item.GetType()].Count == 0)
                    StockpileItems.Remove(item.GetType());
                return true;
            });
            CountItems(item.GetType());
        }

    }
}
