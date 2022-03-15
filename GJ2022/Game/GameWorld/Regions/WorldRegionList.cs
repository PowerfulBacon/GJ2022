using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Game.GameWorld.Regions
{
    public class WorldRegionList
    {

        /// <summary>
        /// The nxn area in which the first level regions are
        /// </summary>
        public const int REGION_PRIMARY_LEVEL_SIZE = 8;

        /// <summary>
        /// nxn regions will exist within a parent region.
        /// For a 2x2 region then with a 4ax4a world (where a is the primary level size) there will be 3 region sets:
        ///  - 16 primary regions
        ///  - 4 parent regions, each holding 4 primary regions
        ///  - 1 super region which holds the 4 parent regions
        ///  (Since there is only 1 super region, there is no need for a super super region in this example.)
        /// </summary>
        public const int REGION_CHILDREN_SIZE = 2;

        /// <summary>
        /// Position based binary list for regions, allows for relatively quick finding of locations at positions as well as infinite non-continuous insertion
        /// requiring O(n) memory with O(log(n)) access time.
        /// </summary>
        public PositionBasedBinaryList<Region> regions = new PositionBasedBinaryList<Region>();

        /// <summary>
        /// Generates the region that contains the provided X,Y world coordinates.
        /// </summary>
        public void GenerateWorldSection(int x, int y)
        {
            //WARNING: Integer divison rounds towards 0.
            //Positive values (8, 16, 24, 32 etc..) should work fine, however it needs to be ensured that (-1 and 1) don't get assigned to the same region,
            //since we don't want region (x, y), (-x, y), (x, -y), (-x, -y) to all be assigned to region (0, 0)
            int regionX = (int)Math.Floor((float)x / REGION_PRIMARY_LEVEL_SIZE);
            int regionY = (int)Math.Floor((float)y / REGION_PRIMARY_LEVEL_SIZE);
            //Check if the region exists already
            if (regions.Get(regionX, regionY) != null)
                return;
            //The region doesn't exist, so we need to generate one

        }

    }
}
