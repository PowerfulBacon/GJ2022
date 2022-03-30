using GJ2022.EntityComponentSystem.Components;
using GJ2022.Game.Power;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;

namespace GJ2022.Game.GameWorld
{

    public static class World
    {

        public static Random Random { get; } = new Random();

        public static int EntitiesCreated { get; set; } = 0;
        public static int EntitiesDestroyed { get; set; } = 0;
        public static int EntitiesGarbageCollected { get; set; } = 0;

        private class IntegerReference
        {

            public IntegerReference(int value)
            {
                Value = value;
            }

            public int Value { get; set; } = 0;
        }

        /// <summary>
        /// A dictionary containing a key value pair
        /// where the key is the ID of a tracking group and the value
        /// is the position based binary list containing the details of the 
        /// things being tracked.
        /// </summary>
        public static Dictionary<string, PositionBasedBinaryList<List<Component>>> TrackedComponentHandlers = new Dictionary<string, PositionBasedBinaryList<List<Component>>>();

        //An integer storing the amount of atmospheric blocking things at this location
        private static PositionBasedBinaryList<IntegerReference> AtmosphericBlockers = new PositionBasedBinaryList<IntegerReference>();

        //======================
        // In range detectors
        //======================

        public static bool HasThingInRange(string thingGroup, int x, int y, int range, BinaryList<List<Component>>.BinaryListValidityCheckDelegate conditionalCheck = null)
        {
            if (!TrackedComponentHandlers.ContainsKey(thingGroup))
                return false;
            return TrackedComponentHandlers[thingGroup].ElementsInRange(x - range, y - range, x + range, y + range, 0, -1, conditionalCheck);
        }

        //======================
        // Spiral Distance Getters
        //======================

        //TODO: Optimise me :)
        public static List<T> GetSpiralThings<T>(string thingGroup, int original_x, int original_y, int range)
            where T : Component
        {
            List<T> output = new List<T>();
            if (!TrackedComponentHandlers.ContainsKey(thingGroup))
                return output;
            for (int r = 0; r <= range; r++)
            {
                //Get all items that are r distance away from (x, y)
                for (int x = original_x - r; x <= original_x + r; x++)
                {
                    for (int y = original_y - r; y <= original_y + r; y += (x == original_x - r || x == original_x + r) ? 1 : r * 2)
                    {
                        List<Component> located = TrackedComponentHandlers[thingGroup].Get(x, y);
                        if (located != null)
                            foreach (T thing in located)
                                output.Add(thing);
                    }
                }
            }
            return output;
        }

        //======================
        // Atmospheric Blockers
        //======================

        public static void AddAtmosphericBlocker(int x, int y, bool updateAtmos = true)
        {
            IntegerReference reference = AtmosphericBlockers.Get(x, y);
            if (reference == null)
            {
                AtmosphericBlockers.Add(x, y, new IntegerReference(1));
                if (updateAtmos)
                    AtmosphericsSystem.Singleton.OnAtmosBlockingChange(x, y, true);
            }
            else
                reference.Value++;
        }

        public static void RemoveAtmosphericBlock(int x, int y, bool updateAtmos = true)
        {
            IntegerReference reference = AtmosphericBlockers.Get(x, y);
            if (reference == null)
                return;
            reference.Value--;
            if (reference.Value == 0)
            {
                AtmosphericBlockers.Remove(x, y);
                if (updateAtmos)
                    AtmosphericsSystem.Singleton.OnAtmosBlockingChange(x, y, false);
            }
        }

        //======================
        // Atmospheric Flow
        //======================

        public static bool AllowsAtmosphericFlow(int x, int y)
        {
            return AtmosphericBlockers.Get(x, y) == null;
        }

        //======================
        // Things
        //======================

        public static List<Component> GetThings(string thingGroup, int x, int y)
        {
            if (!TrackedComponentHandlers.ContainsKey(thingGroup))
                return new List<Component>() { };
            return TrackedComponentHandlers[thingGroup].Get(x, y) ?? new List<Component>() { };
        }

        /// <summary>
        /// Add an pawn to the world list
        /// </summary>
        public static void AddThing(string thingGroup, int x, int y, Component thing)
        {
            if (!TrackedComponentHandlers.ContainsKey(thingGroup))
                TrackedComponentHandlers.Add(thingGroup, new PositionBasedBinaryList<List<Component>>());
            List<Component> located = TrackedComponentHandlers[thingGroup].Get(x, y);
            if (located != null)
                located.Add(thing);
            else
                TrackedComponentHandlers[thingGroup].Add(x, y, new List<Component>() { thing });
        }

        /// <summary>
        /// Remove the pawn from the world list
        /// </summary>
        public static bool RemoveThing(string thingGroup, int x, int y, Component thing)
        {
            if (!TrackedComponentHandlers.ContainsKey(thingGroup))
                return false;
            List<Component> located = TrackedComponentHandlers[thingGroup].Get(x, y);
            if (located == null)
                return false;
            located.Remove(thing);
            if (located.Count == 0)
                TrackedComponentHandlers[thingGroup].Remove(x, y);
            return true;
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
            return IsSolid(position.X, position.Y);
        }

        /// <summary>
        /// Check if a position is solid or not
        /// </summary>
        public static bool IsSolid(int x, int y)
        {
            Turf locatedTurf = GetTurf(x, y);
            //TODO: Proper ISolid + directional solidity
            return locatedTurf != null && locatedTurf.Solid;
        }

        //======================
        // Gravity
        //======================

        public static bool HasGravity(Vector<int> position)
        {
            return HasGravity(position.X, position.Y);
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
