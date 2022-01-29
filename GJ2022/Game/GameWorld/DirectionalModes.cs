using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Game.GameWorld
{
    public enum DirectionalModes
    {
        NONE = 0,
        CARDINAL = 1,
        CARDINAL_DIAGONAL = 2,
        FULL = 3
    }

    public enum Directions
    {
        NONE = 0,
        NORTH = 1,
        EAST = 2,
        SOUTH = 4,
        WEST = 8,
    }

    public static class Direction
    {

        public static int GetDirectionalShift(DirectionalModes mode, Directions dir)
        {
            switch (mode)
            {
                case DirectionalModes.NONE:
                    return 0;
                case DirectionalModes.CARDINAL:
                    switch (dir)
                    {
                        case Directions.SOUTH:
                            return 0;
                        case Directions.NORTH:
                            return 1;
                        case Directions.EAST:
                            return 2;
                        case Directions.WEST:
                            return 3;
                        default:
                            return 0;
                    }
                //Todo: Figure out how byond handles these in texture files
                case DirectionalModes.CARDINAL_DIAGONAL:
                    switch (dir)
                    {
                        default:
                            return 0;
                    }
                case DirectionalModes.FULL:
                    return (int)dir;
                default:
                    return 0;
            }
        }

    }

}
