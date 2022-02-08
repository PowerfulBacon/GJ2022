using GJ2022.Areas;
using GJ2022.Entities;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items;
using GJ2022.Entities.Markers;
using GJ2022.Entities.Pawns;
using GJ2022.Entities.Structures;
using GJ2022.Entities.Turfs;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.Game.GameWorld
{

    public static class World
    {

        private class IntegerReference
        {

            public IntegerReference(int value)
            {
                Value = value;
            }

            public int Value { get; set; } = 0;
        }

        //Dictionary of turfs in the world
        public static PositionBasedBinaryList<Turf> WorldTurfs = new PositionBasedBinaryList<Turf>();

        //Dictionary of areas in the world
        public static PositionBasedBinaryList<Area> WorldAreas = new PositionBasedBinaryList<Area>();

        //Dictionary of markers in the world
        public static PositionBasedBinaryList<Marker> WorldMarkers = new PositionBasedBinaryList<Marker>();

        //Dictionary containing all items in the world at a specified position.
        //When an item moves, it needs to be updated in this list.
        public static PositionBasedBinaryList<List<Item>> WorldItems = new PositionBasedBinaryList<List<Item>>();

        //Dictionary containing all structures in the world
        public static PositionBasedBinaryList<List<Structure>> WorldStructures = new PositionBasedBinaryList<List<Structure>>();

        //Dictionary containing all mobs in the world
        public static PositionBasedBinaryList<List<Pawn>> WorldPawns = new PositionBasedBinaryList<List<Pawn>>();

        //An integer storing the amount of atmospheric blocking things at this location
        private static PositionBasedBinaryList<IntegerReference> AtmosphericBlockers = new PositionBasedBinaryList<IntegerReference>();

        //======================
        // In range detectors
        //======================

        public static bool HasMarkerInRange(int x, int y, int range, BinaryList<Marker>.BinaryListValidityCheckDelegate conditionalCheck = null)
        {
            return WorldMarkers.ElementsInRange(x - range, y - range, x + range, y + range, 0, -1, conditionalCheck);
        }

        public static bool HasTurfInRange(int x, int y, int range, BinaryList<Turf>.BinaryListValidityCheckDelegate conditionalCheck = null)
        {
            return WorldTurfs.ElementsInRange(x - range, y - range, x + range, y + range, 0, -1, conditionalCheck);
        }

        public static bool HasAreaInRange(int x, int y, int range, BinaryList<Area>.BinaryListValidityCheckDelegate conditionalCheck = null)
        {
            return WorldAreas.ElementsInRange(x - range, y - range, x + range, y + range, 0, -1, conditionalCheck);
        }

        public static bool HasItemsInRange(int x, int y, int range, BinaryList<List<Item>>.BinaryListValidityCheckDelegate conditionalCheck = null)
        {
            return WorldItems.ElementsInRange(x - range, y - range, x + range, y + range, 0, -1, conditionalCheck);
        }

        public static bool HasStructuresInRange(int x, int y, int range, BinaryList<List<Structure>>.BinaryListValidityCheckDelegate conditionalCheck = null)
        {
            return WorldStructures.ElementsInRange(x - range, y - range, x + range, y + range, 0, -1, conditionalCheck);
        }

        public static bool HasPawnsInRange(int x, int y, int range, BinaryList<List<Pawn>>.BinaryListValidityCheckDelegate conditionalCheck = null)
        {
            return WorldPawns.ElementsInRange(x - range, y - range, x + range, y + range, 0, -1, conditionalCheck);
        }

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
                        Marker located = WorldMarkers.Get(x, y);
                        if (located != null)
                            output.Add(located);
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
                        Area located = WorldAreas.Get(x, y);
                        if (located != null)
                            output.Add(located);
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
                for (int x = original_x - r; x <= original_x + r; x++)
                {
                    for (int y = original_y - r; y <= original_y + r; y += (x == original_x - r || x == original_x + r) ? 1 : r * 2)
                    {
                        List<Item> located = WorldItems.Get(x, y);
                        if (located != null)
                            output.AddRange(located);
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Get spiral structures, ordered by distance from the origin
        /// </summary>
        public static List<Structure> GetSprialStructure(int original_x, int original_y, int range)
        {
            List<Structure> output = new List<Structure>();
            for (int r = 0; r <= range; r++)
            {
                //Get all items that are r distance away from (x, y)
                for (int x = original_x - r; x <= original_x + r; x++)
                {
                    for (int y = original_y - r; y <= original_y + r; y += (x == original_x - r || x == original_x + r) ? 1 : r * 2)
                    {
                        List<Structure> located = WorldStructures.Get(x, y);
                        if (located != null)
                            output.AddRange(located);
                    }
                }
            }
            return output;
        }

        //======================
        // Atmospheric Blockers
        //======================

        public static void AddAtmosphericBlocker(int x, int y)
        {
            IntegerReference reference = AtmosphericBlockers.Get(x, y);
            if (reference == null)
                AtmosphericBlockers.Add(x, y, new IntegerReference(1));
            else
                reference.Value++;
        }

        public static void RemoveAtmosphericBlock(int x, int y)
        {
            IntegerReference reference = AtmosphericBlockers.Get(x, y);
            if (reference == null)
                return;
            reference.Value --;
            if (reference.Value == 0)
                AtmosphericBlockers.Remove(x, y);
        }

        //======================
        // Atmospheric Flow
        //======================

        public static bool AllowsAtmosphericFlow(int x, int y)
        {
            return AtmosphericBlockers.Get(x, y) != null;
        }

        //======================
        // Pawns
        //======================

        public static List<Pawn> GetPawns(int x, int y)
        {
            return WorldPawns.Get(x, y) ?? new List<Pawn>() { };
        }

        /// <summary>
        /// Add an pawn to the world list
        /// </summary>
        public static void AddPawn(int x, int y, Pawn pawn)
        {
            List<Pawn> located = WorldPawns.Get(x, y);
            if (located != null)
                located.Add(pawn);
            else
                WorldPawns.Add(x, y, new List<Pawn>() { pawn });
        }

        /// <summary>
        /// Remove the pawn from the world list
        /// </summary>
        public static bool RemovePawn(int x, int y, Pawn pawn)
        {
            List<Pawn> located = WorldPawns.Get(x, y);
            if (located == null)
                return false;
            located.Remove(pawn);
            if (located.Count == 0)
                WorldPawns.Remove(x, y);
            return true;
        }

        //======================
        // Structures
        //======================

        public static List<Structure> GetStructures(int x, int y)
        {
            return WorldStructures.Get(x, y) ?? new List<Structure>() { };
        }

        /// <summary>
        /// Add an structure to the world list
        /// </summary>
        public static void AddStructure(int x, int y, Structure structure)
        {
            List<Structure> located = WorldStructures.Get(x, y);
            if (located != null)
                located.Add(structure);
            else
                WorldStructures.Add(x, y, new List<Structure>() { structure });
        }

        /// <summary>
        /// Remove the istructuretem from the world list
        /// </summary>
        public static bool RemoveStructure(int x, int y, Structure structure)
        {
            List<Structure> located = WorldStructures.Get(x, y);
            if (located == null)
                return false;
            located.Remove(structure);
            if (located.Count == 0)
                WorldStructures.Remove(x, y);
            return true;
        }

        //======================
        // All Entities
        //======================

        public static List<Entity> GetEntities(int x, int y)
        {
            List<Entity> output = new List<Entity>();
            output.AddRange(GetItems(x, y));
            output.AddRange(GetStructures(x, y));
            output.AddRange(GetPawns(x, y));
            Marker marker = GetMarker(x, y);
            if (marker != null)
                output.Add(marker);
            Area area = GetArea(x, y);
            if (area != null)
                output.Add(area);
            return output;
        }

        //======================
        // Items
        //======================

        public static List<Item> GetItems(int x, int y)
        {
            return WorldItems.Get(x, y) ?? new List<Item>() { };
        }

        /// <summary>
        /// Add an item to the world list
        /// </summary>
        public static void AddItem(int x, int y, Item item)
        {
            List<Item> located = WorldItems.Get(x, y);
            if (located != null)
                located.Add(item);
            else
                WorldItems.Add(x, y, new List<Item>() { item });
        }

        /// <summary>
        /// Remove the item from the world list
        /// </summary>
        public static bool RemoveItem(int x, int y, Item item)
        {
            List<Item> located = WorldItems.Get(x, y);
            if (located == null)
                return false;
            located.Remove(item);
            if (located.Count == 0)
                WorldItems.Remove(x, y);
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
            return WorldAreas.Get(x, y);
        }

        /// <summary>
        /// Set the area at the specified location
        /// </summary>
        public static void SetArea(int x, int y, Area area)
        {
            if (area == null)
                WorldAreas.Remove(x, y);
            else
                WorldAreas.Add(x, y, area);
        }

        //======================
        // Markers
        //======================

        /// <summary>
        /// Get the turf at the specified location.
        /// </summary>
        public static Marker GetMarker(int x, int y)
        {
            return WorldMarkers.Get(x, y);
        }

        /// <summary>
        /// Set the turfs
        /// </summary>
        public static void SetMarker(int x, int y, Marker marker)
        {
            if (marker == null)
                WorldMarkers.Remove(x, y);
            else
                WorldMarkers.Add(x, y, marker);
        }

        //======================
        // Turfs
        //======================

        /// <summary>
        /// Get the turf at the specified location.
        /// </summary>
        public static Turf GetTurf(int x, int y)
        {
            return WorldTurfs.Get(x, y);
        }

        /// <summary>
        /// Set the turfs
        /// </summary>
        public static void SetTurf(int x, int y, Turf turf)
        {
            if (turf == null)
                WorldTurfs.Remove(x, y);
            else
                WorldTurfs.Add(x, y, turf);
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
