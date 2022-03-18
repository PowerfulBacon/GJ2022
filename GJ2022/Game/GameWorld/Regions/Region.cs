using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Game.GameWorld.Regions
{
    /// <summary>
    /// Region system:
    /// ========
    /// 
    /// A region determines a section of the world in which all points are accessible to each other.
    /// 
    /// The world will be broken down into 8x8 chunks and each one designated regions.
    /// If there is non passable turfs within any of the 8x8 chunks that split the region into pieces, then many regions can be generated for 1 of these.
    /// 
    /// Region Rules:
    /// =========
    ///  - For each point within a region, there must exist a valid path between those 2 points without leaving that region
    ///  - A region shares at least 1 superparent with any region that can be accessed by it.
    ///  - Regions can be only children as long as they have a parent up high enough that has 2 children.
    ///  
    /// Region Properties:
    /// =========
    ///  - Checking if a valid path exists between 2 points can be done by getting the regions of the 2 points and seeing if they have a shared super-parent.
    ///  
    /// Region Functionality:
    /// =========
    /// When a new region is created we need to create parent regions if neccessary.
    /// To do this, we need to locate any regions that we are attached to directly.
    /// We then need to calculate the level of our shared parent.
    /// We then traverse up both trees until we hit the top level, or the level of the shared parent.
    /// We then create parents where necessary until we hit the shared parent.
    /// Both nodes should now have the same superparent
    /// 
    /// When removing a region:
    /// =========
    /// Check if we have created a new region by blocking 2 nodes from each other within a region.
    /// Create a new region if we need, setting the tiles that are in this region to be adjacent to this region
    /// Locate the adjacent regions to the updated region
    /// Calculate which adjacencies no longer exist but are represented in the data structure
    /// Seperate the tree structure
    /// </summary>
    public class Region
    {

        private static int count = 0;

        //The unique ID of this region, for debugging purposes
        public int Id { get; } = count++;

        //The parent region
        public Region Parent { get; set; }

        //The region height (How many parent's up are we)
        //A height of 0 indicates that this region is the parent of actual world positions.
        public int Height { get; }

        //The X position of the region
        //The world coordinated of the left divided by the primary level size
        public int X { get; }

        //The Y position of the region
        //The world coordinated of the bottom divided by the primary level size
        public int Y { get; }

        //A list of adjacent regions to this region
        //This is likely to be null for any region that isn't a primary
        //level region.
        //If this region isn't a primary region, then the adjacent regions is calculated
        //by performing the union operation on the children's linked regions parents.
        //TODO
        public List<Region> AdjacentRegions { get; set; }

        /// <summary>
        /// Instantiates a region based on an X and Y position, setting the region's height
        /// to the height parameter
        /// </summary>
        /// <param name="x">The X value of the bottom left part of region</param>
        /// <param name="y">The Y value of the bottom left part of the region</param>
        /// <param name="height">The height of the node in the tree data structure</param>
        public Region(int x, int y, int height = 0)
        {
            X = x;
            Y = y;
            Height = height;
        }

        /// <summary>
        /// Instantiate an instance of region based on a parent instance.
        /// </summary>
        public Region(Region parent)
        {
            Parent = parent;
            Height = parent.Height - 1;
        }

        /// <summary>
        /// Determines if a path exists between 2 regions.
        /// O(log(n)), extremely lightweight compared to checking all possible nodes
        /// especially at long distances.
        /// </summary>
        /// <param name="other">The region to determine if there is a path to</param>
        /// <returns>Returns true or false depending on if there exists a theoretical path between 2 points.</returns>
        public bool HasPath(Region other)
        {
            //Set the initial tracking pointers
            Region currentLeft = this;
            Region currentRight = other;
            //Bad inputs
            if (currentLeft == null || currentRight == null)
                return false;
            //Move the regions until their heights are matching
            //HeightA < HeightB, so move A upwards
            while (currentLeft.Height < currentRight.Height)
            {
                //Left has no parent, they cannot be connected
                if (currentLeft.Parent == null)
                    return false;
                //Move the left pointer up to the parent
                currentLeft = currentLeft.Parent;
            }
            //HeightA > HeightB, so move B upwards
            while (currentLeft.Height > currentRight.Height)
            {
                //Left has no parent, they cannot be connected
                if (currentRight.Parent == null)
                    return false;
                //Move the left pointer up to the parent
                currentRight = currentRight.Parent;
            }
            //Now the heights are equal, check to see if they are the same and recursively move up
            //until they are the same, or we reach the end of the tree structure.
            //If we find a shared parent, this means there is a valid path between 2 regions.
            do
            {
                //We found a shared parent!
                if (currentLeft == currentRight)
                    return true;
                //Move upwards
                currentLeft = currentLeft.Parent;
                currentRight = currentRight.Parent;
            }
            while (currentLeft != null && currentRight != null);
            //No shared parent was located, regions are not connected via an open path.
            return false;
        }

    }
}
