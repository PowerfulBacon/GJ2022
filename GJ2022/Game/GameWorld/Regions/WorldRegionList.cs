//#define REGION_LOGGING

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
        /// Update a node to no longer be solid
        /// </summary>
        public void SetNodeNonSolid(int x, int y)
        {
            //Calculate if we need to do anything at all within this region
            //If we are now seperated from the rest of our region create
            //a new region to represent this area and calculate adjacencies
        }

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
#if REGION_LOGGING
            Log.WriteLine($"Generating region ({regionX}, {regionY})");
#endif
            //Check if the region exists already
            if (regions.Get(x, y) != null)
                return;
            //The region doesn't exist, so we need to generate one
            bool[,] processedNodes = new bool[REGION_PRIMARY_LEVEL_SIZE, REGION_PRIMARY_LEVEL_SIZE];
            //Check each node and flood fill a region if necessary
            for (int nodeX = 0; nodeX < REGION_PRIMARY_LEVEL_SIZE; nodeX++)
            {
                for (int nodeY = 0; nodeY < REGION_PRIMARY_LEVEL_SIZE; nodeY++)
                {
                    //This node has already been assigned a region
                    if (processedNodes[nodeX, nodeY])
                        continue;
                    //Get the real world coordinates
                    int realX = regionX * REGION_PRIMARY_LEVEL_SIZE + nodeX;
                    int realY = regionY * REGION_PRIMARY_LEVEL_SIZE + nodeY;
                    //This position is solid, ignore (We don't need to update processedNodes, since we will never again access it)
                    if (World.IsSolid(realX, realY))
                        continue;
                    //Create a new region and flood fill outwards
                    Region createdRegion = new Region(regionX, regionY);
#if REGION_LOGGING
                    Log.WriteLine($"Created new region {createdRegion.Id} at ({regionX}, {regionY})");
#endif
                    //Have the position join this region
                    regions.Add(realX, realY, createdRegion);
                    //During this process, get the adjacent regions so we can match up parents
                    List<Region> adjacentRegions = new List<Region>();
                    //Processing queue
                    Queue<Vector<int>> toProcess = new Queue<Vector<int>>();
                    toProcess.Enqueue(new Vector<int>(nodeX, nodeY));
                    //While we still have places to floor fill
                    while (toProcess.Count > 0)
                    {
                        //Dequeue
                        Vector<int> relativePosition = toProcess.Dequeue();
                        //If current node is already processed, skip
                        if (processedNodes[relativePosition.X, relativePosition.Y])
                            continue;
                        //Calculate the world position of the current node
                        Vector<int> worldPosition = new Vector<int>(regionX * REGION_PRIMARY_LEVEL_SIZE, regionY * REGION_PRIMARY_LEVEL_SIZE) + relativePosition;
                        //Set node processed
                        processedNodes[relativePosition.X, relativePosition.Y] = true;
                        //If current node is a wall, skip
                        if (World.IsSolid(worldPosition.X, worldPosition.Y))
                            continue;
                        //Join the region
                        regions.Add(worldPosition.X, worldPosition.Y, createdRegion);
                        //Check if we have any adjacent regions
                        Region adjacent;
                        //Add adjacent nodes (Assuming they are within bounds)
                        if (relativePosition.X < REGION_PRIMARY_LEVEL_SIZE - 1)
                            toProcess.Enqueue(new Vector<int>(relativePosition.X + 1, relativePosition.Y));
                        else if ((adjacent = regions.Get(worldPosition.X + 1, worldPosition.Y)) != null && !adjacentRegions.Contains(adjacent))
                            adjacentRegions.Add(adjacent);
                        if (relativePosition.X > 0)
                            toProcess.Enqueue(new Vector<int>(relativePosition.X - 1, relativePosition.Y));
                        else if ((adjacent = regions.Get(worldPosition.X - 1, worldPosition.Y)) != null && !adjacentRegions.Contains(adjacent))
                            adjacentRegions.Add(adjacent);
                        if (relativePosition.Y < REGION_PRIMARY_LEVEL_SIZE - 1)
                            toProcess.Enqueue(new Vector<int>(relativePosition.X, relativePosition.Y + 1));
                        else if ((adjacent = regions.Get(worldPosition.X, worldPosition.Y + 1)) != null && !adjacentRegions.Contains(adjacent))
                            adjacentRegions.Add(adjacent);
                        if (relativePosition.Y > 0)
                            toProcess.Enqueue(new Vector<int>(relativePosition.X, relativePosition.Y - 1));
                        else if ((adjacent = regions.Get(worldPosition.X, worldPosition.Y - 1)) != null && !adjacentRegions.Contains(adjacent))
                            adjacentRegions.Add(adjacent);
                    }
                    //Now we have assigned all tiles within our region to be associated to this region
                    //We now need to calculate parent regions with all adjacent regions
                    foreach (Region adjacentRegion in adjacentRegions)
                    {
#if REGION_LOGGING
                        Log.WriteLine($"Joining region {createdRegion.Id} to {adjacentRegion.Id}");
#endif
                        //The level of the shared parent
                        int sharedParentLevel = GetSharedParentLevel(createdRegion, adjacentRegion);
#if REGION_LOGGING
                        Log.WriteLine($"Shared parent level: {sharedParentLevel}");
#endif
                        //Since these regions are adjacent, they should share a parent at the shared parent level.
                        //Get pointers for the current region
                        Region leftParent = createdRegion;
                        Region rightParent = adjacentRegion;
                        //Iterate upwards, creating parent regions where none exist
                        for (int level = 1; level <= sharedParentLevel - 1; level++)
                        {
                            //Create the parent if either are null
                            if (leftParent.Parent == null)
                            {
                                //Parent position is the integer division result of any
                                //child position divided by the region child size
                                //We can work this out by doing integer division with the position of
                                //The top level child and region children size ^ depth
                                leftParent.Parent = new Region(
                                    createdRegion.X / (int)Math.Pow(REGION_CHILDREN_SIZE, level),
                                    createdRegion.Y / (int)Math.Pow(REGION_CHILDREN_SIZE, level),
                                    level);
#if REGION_LOGGING
                                Log.WriteLine($"Created new parent node for {leftParent.Id}, parent node {leftParent.Parent.Id} at depth {level}");
                                Log.WriteLine($"Joined {leftParent.Id} --> {leftParent.Parent.Id}");
#endif
                            }
                            if (rightParent.Parent == null)
                            {
                                rightParent.Parent = new Region(
                                    adjacentRegion.X / (int)Math.Pow(REGION_CHILDREN_SIZE, level),
                                    adjacentRegion.Y / (int)Math.Pow(REGION_CHILDREN_SIZE, level),
                                    level);
#if REGION_LOGGING
                                Log.WriteLine($"Created new parent node for {rightParent.Id}, parent node {rightParent.Parent.Id} at depth {level}");
                                Log.WriteLine($"Joined {rightParent.Id} --> {rightParent.Parent.Id}"); t depth { level}
                                ");
                                Log.WriteLine($"Joined {leftParent.Id} --> {leftParent.Parent.Id}");
#endif
                            }
                            //Get the left and right parent
                            leftParent = leftParent.Parent;
                            rightParent = rightParent.Parent;
                        }
                        //Now we are at a shared parent level, check if anything exists
                        //If so, then attach both to the shared parent
                        //If not, then create a new shared parent and attach both to it
                        if (rightParent.Parent != null)
                        {
                            leftParent.Parent = rightParent.Parent;
#if REGION_LOGGING
                            Log.WriteLine($"Joined {leftParent.Id} --> {rightParent.Parent.Id}");
#endif
                        }
                        else
                        {
                            if (leftParent.Parent == null)
                            {
                                leftParent.Parent = new Region(
                                    createdRegion.X / (int)Math.Pow(REGION_CHILDREN_SIZE, sharedParentLevel),
                                    createdRegion.Y / (int)Math.Pow(REGION_CHILDREN_SIZE, sharedParentLevel),
                                    sharedParentLevel);
#if REGION_LOGGING
                                Log.WriteLine($"Created new parent node {leftParent.Parent.Id} at depth {sharedParentLevel}");
                                Log.WriteLine($"Joined {leftParent.Id} --> {leftParent.Parent.Id}");
#endif
                            }
                            rightParent.Parent = leftParent.Parent;
#if REGION_LOGGING
                            Log.WriteLine($"Joined {rightParent.Id} --> {leftParent.Parent.Id}");
#endif
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the level at which 2 nodes would in theory have a shared parent.
        /// </summary>
        /// <param name="region">The first region</param>
        /// <param name="sibling">The second region</param>
        /// <returns>An integer value representing the theoretical level that 2 regions would have a shared parent on. If 2 nodes are siblings, 1 is returned. If the nodes are the same 0 is returned.</returns>
        public int GetSharedParentLevel(Region region, Region sibling)
        {
            return Math.Max(GetSharedParentLevel(region.X, sibling.X), GetSharedParentLevel(region.Y, sibling.Y));
        }

        /// <summary>
        /// Recursive algorithm to calculate the parent level of regions.
        /// Recursion depth should never pass 1 in theory, however hits 2 for (1,0) and (0,1)
        /// 
        /// I am sure there is a way to mathematically optimise this but I am not sure how
        /// </summary>
        public int GetSharedParentLevel(int x, int y)
        {
            if (x == y)
                return 0;
            int logx = (int)Math.Ceiling(Math.Log(x + 1, 2));
            int logy = (int)Math.Ceiling(Math.Log(y + 1, 2));
            int powx = (int)Math.Pow(2, logx - 1);
            int powy = (int)Math.Pow(2, logy - 1);
            int maxpow = Math.Max(powx, powy);
            //Bottom Left
            if (y >= maxpow && x < maxpow)
            {
                return logy;
            }
            //Top Right
            else if (x >= maxpow && y < maxpow)
            {
                return logx;
            }
            //Bottom Right, translate so that we aren't the top right anymore
            else
            {
                return GetSharedParentLevel(x - powx, y - powy);
            }
        }

    }
}
