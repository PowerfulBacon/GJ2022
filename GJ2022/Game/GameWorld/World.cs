using GJ2022.Areas;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items;
using GJ2022.Entities.Turfs;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.Game.GameWorld
{

    public static class World
    {

        //Dictionary of turfs in the world
        public static Dictionary<Vector<int>, Turf> WorldTurfs = new Dictionary<Vector<int>, Turf>();

        //Dictionary of areas in the world
        public static Dictionary<Vector<int>, Area> WorldAreas = new Dictionary<Vector<int>, Area>();

        //Dictionary containing all items in the world at a specified position.
        //When an item moves, it needs to be updated in this list.
        public static Dictionary<Vector<int>, List<Item>> WorldItems = new Dictionary<Vector<int>, List<Item>>();

        /// <summary>
        /// Get spiral items, ordered by distance from the origin
        /// </summary>
        public static List<Area> GetSprialAreas(int original_x, int original_y, int range)
        {
            List<Area> output = new List<Area>();
            for (int r = 0; r <= range; r++)
            {
                //Get all items that are r distance away from (x, y)
                for (int x = original_x - r; x <= original_x + r; x++)
                {
                    for (int y = original_y - r; y <= original_y + r; y += (x == original_x - r || x == original_x + r) ? 1 : r * 2)
                    {
                        Vector<int> targetPosition = new Vector<int>(x, y);
                        if (WorldAreas.ContainsKey(targetPosition))
                            output.Add(WorldAreas[targetPosition]);
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Get spiral items, ordered by distance from the origin
        /// </summary>
        public static List<Item> GetSprialItems(int original_x, int original_y, int range)
        {
            List<Item> output = new List<Item>();
            for (int r = 0; r <= range; r++)
            {
                //Get all items that are r distance away from (x, y)
                for (int x = original_x - r; x <= original_x + r; x ++)
                {
                    for (int y = original_y - r; y <= original_y + r; y += (x == original_x - r || x == original_x + r) ? 1 : r * 2)
                    {
                        Vector<int> targetPosition = new Vector<int>(x, y);
                        if (WorldItems.ContainsKey(targetPosition))
                            output.AddRange(WorldItems[targetPosition]);
                    }
                }
            }
            return output;
        }

        public static List<Item> GetItems(int x, int y)
        {
            Vector<int> targetPosition = new Vector<int>(x, y);
            if (WorldItems.ContainsKey(targetPosition))
                return WorldItems[targetPosition];
            return new List<Item>() { };
        }

        /// <summary>
        /// Add an item to the world list
        /// </summary>
        public static void AddItem(int x, int y, Item item)
        {
            Vector<int> targetPosition = new Vector<int>(x, y);
            if (WorldItems.ContainsKey(targetPosition))
                WorldItems[targetPosition].Add(item);
            else
                WorldItems.Add(targetPosition, new List<Item>() { item });
        }

        /// <summary>
        /// Remove the item from the world list
        /// </summary>
        public static bool RemoveItem(int x, int y, Item item)
        {
            Vector<int> targetPosition = new Vector<int>(x, y);
            if (!WorldItems.ContainsKey(targetPosition))
                return false;
            WorldItems[targetPosition].Remove(item);
            if (WorldItems[targetPosition].Count == 0)
                WorldItems.Remove(targetPosition);
            return true;
        }

        /// <summary>
        /// Get the area at the specified location.
        /// </summary>
        public static Area GetArea(int x, int y)
        {
            Vector<int> targetPosition = new Vector<int>(x, y);
            if (WorldAreas.ContainsKey(targetPosition))
                return WorldAreas[targetPosition];
            return null;
        }

        /// <summary>
        /// Set the area at the specified location
        /// </summary>
        public static void SetArea(int x, int y, Area area)
        {
            Vector<int> targetPosition = new Vector<int>(x, y);
            if (area == null)
                WorldAreas.Remove(targetPosition);
            else
                WorldAreas.Add(targetPosition, area);
        }

        /// <summary>
        /// Get the turf at the specified location.
        /// </summary>
        public static Turf GetTurf(int x, int y)
        {
            Vector<int> targetPosition = new Vector<int>(x, y);
            if (WorldTurfs.ContainsKey(targetPosition))
                return WorldTurfs[targetPosition];
            return null;
        }

        /// <summary>
        /// Set the turfs
        /// </summary>
        public static void SetTurf(int x, int y, Turf turf)
        {
            Vector<int> targetPosition = new Vector<int>(x, y);
            if (turf == null)
                WorldTurfs.Remove(targetPosition);
            else
                WorldTurfs.Add(targetPosition, turf);
        }

        public static bool IsSolid(Vector<int> position)
        {
            return IsSolid(position[0], position[1]);
        }

        /// <summary>
        /// Check if a position is solid or not
        /// </summary>
        public static bool IsSolid(int x, int y)
        {
            Turf locatedTurf = GetTurf(x, y);
            //TODO: Proper ISolid + directional solidity
            return locatedTurf != null && locatedTurf is ISolid;
        }

    }

}
