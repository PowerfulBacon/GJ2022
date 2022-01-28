using GJ2022.Areas;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items;
using GJ2022.Entities.Markers;
using GJ2022.Entities.Structures;
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

        //Dictionary of markers in the world
        public static Dictionary<Vector<int>, Marker> WorldMarkers = new Dictionary<Vector<int>, Marker>();

        //Dictionary containing all items in the world at a specified position.
        //When an item moves, it needs to be updated in this list.
        public static Dictionary<Vector<int>, List<Item>> WorldItems = new Dictionary<Vector<int>, List<Item>>();

        //Dictionary containing all structures in the world
        public static Dictionary<Vector<int>, List<Structure>> WorldStructures = new Dictionary<Vector<int>, List<Structure>>();

        //======================
        // Spiral Distance Getters
        //======================

        /// <summary>
        /// Get spiral markers, ordered by distance from the origin
        /// </summary>
        public static List<Marker> GetSprialMarkers(int original_x, int original_y, int range)
        {
            List<Marker> output = new List<Marker>();
            for (int r = 0; r <= range; r++)
            {
                //Get all items that are r distance away from (x, y)
                for (int x = original_x - r; x <= original_x + r; x++)
                {
                    for (int y = original_y - r; y <= original_y + r; y += (x == original_x - r || x == original_x + r) ? 1 : r * 2)
                    {
                        Vector<int> targetPosition = new Vector<int>(x, y);
                        if (WorldMarkers.ContainsKey(targetPosition))
                            output.Add(WorldMarkers[targetPosition]);
                    }
                }
            }
            return output;
        }

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

        //======================
        // Structures
        //======================

        public static List<Structure> GetStructures(int x, int y)
        {
            Vector<int> targetPosition = new Vector<int>(x, y);
            if (WorldStructures.ContainsKey(targetPosition))
                return WorldStructures[targetPosition];
            return new List<Structure>() { };
        }

        /// <summary>
        /// Add an structure to the world list
        /// </summary>
        public static void AddStructure(int x, int y, Structure structure)
        {
            Vector<int> targetPosition = new Vector<int>(x, y);
            if (WorldStructures.ContainsKey(targetPosition))
                WorldStructures[targetPosition].Add(structure);
            else
                WorldStructures.Add(targetPosition, new List<Structure>() { structure });
        }

        /// <summary>
        /// Remove the istructuretem from the world list
        /// </summary>
        public static bool RemoveStructure(int x, int y, Structure structure)
        {
            Vector<int> targetPosition = new Vector<int>(x, y);
            if (!WorldStructures.ContainsKey(targetPosition))
                return false;
            WorldStructures[targetPosition].Remove(structure);
            if (WorldStructures[targetPosition].Count == 0)
                WorldStructures.Remove(targetPosition);
            return true;
        }

        //======================
        // Items
        //======================

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

        //======================
        // Areas
        //======================

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

        //======================
        // Markers
        //======================

        /// <summary>
        /// Get the turf at the specified location.
        /// </summary>
        public static Marker GetMarker(int x, int y)
        {
            Vector<int> targetPosition = new Vector<int>(x, y);
            if (WorldMarkers.ContainsKey(targetPosition))
                return WorldMarkers[targetPosition];
            return null;
        }

        /// <summary>
        /// Set the turfs
        /// </summary>
        public static void SetMarker(int x, int y, Marker marker)
        {
            Vector<int> targetPosition = new Vector<int>(x, y);
            if (marker == null)
                WorldMarkers.Remove(targetPosition);
            else
                WorldMarkers.Add(targetPosition, marker);
        }

        //======================
        // Turfs
        //======================

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

        //======================
        // Surrounded detected
        //======================

        public static bool IsLocationFullyEnclosed(int x, int y)
        {
            return IsSolid(x, y + 1) && IsSolid(x + 1, y) && IsSolid(x, y - 1) && IsSolid(x - 1, y);
        }

        public static Vector<float>? GetFreeAdjacentLocation(int x, int y)
        {
            if (!IsSolid(x, y + 1))
                return new Vector<float>(x, y + 1);
            if (!IsSolid(x + 1, y))
                return new Vector<float>(x + 1, y);
            if (!IsSolid(x, y - 1))
                return new Vector<float>(x, y - 1);
            if (!IsSolid(x - 1, y))
                return new Vector<float>(x - 1, y + 1);
            return null;
        }

        //======================
        // Solidity
        //======================

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

        //======================
        // Gravity
        //======================

        public static bool HasGravity(Vector<int> position)
        {
            return HasGravity(position[0], position[1]);
        }

        public static bool HasGravity(int x, int y)
        {
            //Structure = gravity
            if (GetStructures(x, y).Count > 0)
                return true;
            //Turf = gravity
            if (GetTurf(x, y) != null)
                return true;
            //Holding onto a turf = gravity
            if (GetTurf(x + 1, y) != null || GetTurf(x, y + 1) != null || GetTurf(x - 1, y) != null || GetTurf(x, y - 1) != null ||
                GetTurf(x + 1, y + 1) != null || GetTurf(x - 1, y + 1) != null || GetTurf(x - 1, y - 1) != null || GetTurf(x + 1, y - 1) != null)
                return true;
            //No gravity :(
            return false;
        }

    }

}
