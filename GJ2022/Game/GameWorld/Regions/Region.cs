using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Game.GameWorld.Regions
{
    public class Region
    {

        //The parent region
        public Region Parent { get; private set; }

        //The region height (How many parent's up are we)
        //A height of 0 indicates that this region is the parent of actual world positions.
        public int Height { get; }

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
