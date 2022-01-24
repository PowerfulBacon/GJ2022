using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Turfs;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.Game.GameWorld
{

    public static class World
    {

        //Array of turfs in the world
        public static Dictionary<Vector<float>, Turf> WorldTurfs = new Dictionary<Vector<float>, Turf>();

        /// <summary>
        /// Get the turf at the specified location.
        /// </summary>
        public static Turf GetTurf(int x, int y)
        {
            Vector<float> targetPosition = new Vector<float>(x, y);
            if (WorldTurfs.ContainsKey(targetPosition))
                return WorldTurfs[targetPosition];
            return null;
        }

        /// <summary>
        /// Set the turfs
        /// </summary>
        public static void SetTurf(int x, int y, Turf turf)
        {
            Vector<float> targetPosition = new Vector<float>(x, y);
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
            return locatedTurf == null || !(locatedTurf is ISolid);
        }

    }

}
