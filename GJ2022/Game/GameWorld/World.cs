using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Turfs;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.Game.GameWorld
{

    public static class World
    {

        //Array of turfs in the world
        public static Dictionary<Vector, Turf> WorldTurfs = new Dictionary<Vector, Turf>();

        /// <summary>
        /// Get the turf at the specified location.
        /// </summary>
        public static Turf GetTurf(int x, int y)
        {
            Vector targetPosition = new Vector(x, y);
            if (WorldTurfs.ContainsKey(targetPosition))
                return WorldTurfs[targetPosition];
            return null;
        }

        /// <summary>
        /// Set the turfs
        /// </summary>
        public static void SetTurf(int x, int y, Turf turf)
        {
            Vector targetPosition = new Vector(x, y);
            if (turf == null)
                WorldTurfs.Remove(targetPosition);
            else
                WorldTurfs.Add(targetPosition, turf);
        }

    }

}
