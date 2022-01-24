using GJ2022.Areas;
using GJ2022.Entities.Items;
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
        public static Dictionary<Type, Dictionary<StockpileArea, List<Item>>> StockpileItems { get; } = new Dictionary<Type, Dictionary<StockpileArea, List<Item>>>();

        public static void AddItem()
        {

        }

    }
}
