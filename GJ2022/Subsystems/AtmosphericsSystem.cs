using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GJ2022.Atmospherics;
using GJ2022.Atmospherics.Block;
using GJ2022.Entities.Turfs;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;
using GLFW;

namespace GJ2022.Subsystems
{

    /// <summary>
    /// When a turf is created we need to check all the surrounding tiles to see if they are enclosed.
    /// When a turf is destoyed, we need to merge all atmospheres that were touching it
    /// </summary>
    public class AtmosphericsSystem : Subsystem
    {

        //Atmospheric system singleton
        public static AtmosphericsSystem Singleton { get; } = new AtmosphericsSystem();

        //20 times per second should be more than enough
        public override int sleepDelay => 50;

        //Set the flags
        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NONE;

        //Store a list of any turf atmospheric flow changes, use this to recalculate atmospheric blocks
        private PositionBasedBinaryList<Turf> processingDeltas = new PositionBasedBinaryList<Turf>();

        public override void Fire(Window window)
        {
            while (processingDeltas.HasElements)
            {
                //Take the first element out and begin processing
                Turf first = processingDeltas.TakeFirst();
                //If the first is null, something went wrong
                //TODO: (Null should be processed when a world turf is set to null)
                if (first == null)
                    throw new System.Exception("Error: Turf in the atmospherics delta queue was null");
                //Turf was already destroyed
                if (first.Destroyed)
                    continue;
                //Process the turf
                ProcessDelta(first.X, first.Y, first);
            }
        }

        //Turf => null
        public void OnTurfDestroyed(Turf destroyedTurf)
        {
            List<AtmosphericBlock> blocksToClear = new List<AtmosphericBlock>();
            //If we didn't allow atmospheric flow, collect all surrounding atmospheres and merge them with null
            if (!destroyedTurf.AllowAtmosphericFlow)
            {
                AtmosphericBlock atmos;
                if ((atmos = World.GetTurf(destroyedTurf.X, destroyedTurf.Y + 1).Atmosphere) != null && !blocksToClear.Contains(atmos))
                    blocksToClear.Add(atmos);
                if ((atmos = World.GetTurf(destroyedTurf.X + 1, destroyedTurf.Y).Atmosphere) != null && !blocksToClear.Contains(atmos))
                    blocksToClear.Add(atmos);
                if ((atmos = World.GetTurf(destroyedTurf.X, destroyedTurf.Y - 1).Atmosphere) != null && !blocksToClear.Contains(atmos))
                    blocksToClear.Add(atmos);
                if ((atmos = World.GetTurf(destroyedTurf.X - 1, destroyedTurf.Y).Atmosphere) != null && !blocksToClear.Contains(atmos))
                    blocksToClear.Add(atmos);
            }
            else
            {
                blocksToClear.Add(destroyedTurf.Atmosphere);
            }
            //Merge atmospheres with null
            foreach(AtmosphericBlock block in blocksToClear)
                block.MergeAtmosphericBlockInto(null, new Vector<int>(destroyedTurf.X, destroyedTurf.Y));
        }

        //Turf => Turf
        public void OnTurfChanged(Turf oldTurf, Turf newTurf)
        {
            //Atmosflow allowed => allowed - inherit the old turfs atmosphere
            if (oldTurf.AllowAtmosphericFlow && newTurf.AllowAtmosphericFlow)
            {
                //Swap the old turf for the new turf in the atmosphere list
                oldTurf.Atmosphere?.ChangeTurf(oldTurf, newTurf);
            }
            //Atmosflow allowed => disallowed - Check to see if we need to split the atmosphere area in half (Add surrounding tiles to processing queue)
            else if (oldTurf.AllowAtmosphericFlow && !newTurf.AllowAtmosphericFlow)
            {
                //TODO: this one
                //Set the atmosphere to be outdated
                if(oldTurf.Atmosphere != null)
                    oldTurf.Atmosphere.Outdated = true;
                //The new atmosphere created by processing delta needs to get the air from the old atmosphere
                oldTurf.Atmosphere?.RemoveTurf(oldTurf);
                Turf turf;
                if ((turf = World.GetTurf(newTurf.X + 1, newTurf.Y)) != null && turf.AllowAtmosphericFlow)
                    processingDeltas.Add(newTurf.X + 1, newTurf.Y, turf);
                if ((turf = World.GetTurf(newTurf.X, newTurf.Y + 1)) != null && turf.AllowAtmosphericFlow)
                    processingDeltas.Add(newTurf.X, newTurf.Y + 1, turf);
                if ((turf = World.GetTurf(newTurf.X - 1, newTurf.Y)) != null && turf.AllowAtmosphericFlow)
                    processingDeltas.Add(newTurf.X - 1, newTurf.Y, turf);
                if ((turf = World.GetTurf(newTurf.X, newTurf.Y - 1)) != null && turf.AllowAtmosphericFlow)
                    processingDeltas.Add(newTurf.X, newTurf.Y - 1, turf);
            }
            //Atmosflow disallowed => allowed - Merge surrounding atmospheres
            //TODO: Investigate pressure sometimes dropping to 0
            else if (!oldTurf.AllowAtmosphericFlow && newTurf.AllowAtmosphericFlow)
            {
                //Collect all surrounding atmospheres
                List<AtmosphericBlock> blocksToMerge = new List<AtmosphericBlock>();
                AtmosphericBlock atmos;
                Turf turf;
                bool spaceAdjacent = false;
                if ((atmos = (turf = World.GetTurf(newTurf.X, newTurf.Y + 1))?.Atmosphere) != null && !blocksToMerge.Contains(atmos))
                    blocksToMerge.Add(atmos);
                spaceAdjacent = spaceAdjacent || turf == null || (turf.AllowAtmosphericFlow && atmos == null);
                if ((atmos = (turf = World.GetTurf(newTurf.X + 1, newTurf.Y))?.Atmosphere) != null && !blocksToMerge.Contains(atmos))
                    blocksToMerge.Add(atmos);
                spaceAdjacent = spaceAdjacent || turf == null || (turf.AllowAtmosphericFlow && atmos == null);
                if ((atmos = (turf = World.GetTurf(newTurf.X, newTurf.Y - 1))?.Atmosphere) != null && !blocksToMerge.Contains(atmos))
                    blocksToMerge.Add(atmos);
                spaceAdjacent = spaceAdjacent || turf == null || (turf.AllowAtmosphericFlow && atmos == null);
                if ((atmos = (turf = World.GetTurf(newTurf.X - 1, newTurf.Y))?.Atmosphere) != null && !blocksToMerge.Contains(atmos))
                    blocksToMerge.Add(atmos);
                spaceAdjacent = spaceAdjacent || turf == null || (turf.AllowAtmosphericFlow && atmos == null);
                //Merge all surrounding atmospheres into 1
                if (blocksToMerge.Count > 0)
                {
                    if (!spaceAdjacent)
                    {
                        //Merge everything into this atmos
                        AtmosphericBlock mergeMaster = blocksToMerge.ElementAt(0);
                        //Perform merging on any additional atmospheric blocks
                        for (int i = 1; i < blocksToMerge.Count; i++)
                        {
                            mergeMaster.MergeAtmosphericBlockInto(blocksToMerge[i], new Vector<int>(newTurf.X, newTurf.Y));
                        }
                        //Add our turf to that block
                        mergeMaster.AddTurf(newTurf);
                    }
                    else
                    {
                        //Vent all atmospheric blocks
                        for (int i = 0; i < blocksToMerge.Count; i++)
                        {
                            blocksToMerge[i].MergeAtmosphericBlockInto(null, new Vector<int>(newTurf.X, newTurf.Y));
                        }
                    }
                }
                else if(!spaceAdjacent)
                {
                    //Create a new atmosphere for this tile (There were no surrounding atmospheric blocks we could join)
                    new AtmosphericBlock(newTurf);
                }
            }
            //Atmosflow disallowed => disallowed - Do nothing (Both atmospheres are null)
        }

        //null => Turf
        public void OnTurfCreated(Turf createdTurf)
        {
            //If the new turf doesn't allow atmospheric flow add surrounding tiles to the processing queue
            if (!createdTurf.AllowAtmosphericFlow)
            {
                Turf turf;
                if((turf = World.GetTurf(createdTurf.X + 1, createdTurf.Y)) != null && turf.AllowAtmosphericFlow)
                    processingDeltas.Add(createdTurf.X + 1, createdTurf.Y, turf);
                if ((turf = World.GetTurf(createdTurf.X, createdTurf.Y + 1)) != null && turf.AllowAtmosphericFlow)
                    processingDeltas.Add(createdTurf.X, createdTurf.Y + 1, turf);
                if ((turf = World.GetTurf(createdTurf.X - 1, createdTurf.Y)) != null && turf.AllowAtmosphericFlow)
                    processingDeltas.Add(createdTurf.X - 1, createdTurf.Y, turf);
                if ((turf = World.GetTurf(createdTurf.X, createdTurf.Y - 1)) != null && turf.AllowAtmosphericFlow)
                    processingDeltas.Add(createdTurf.X, createdTurf.Y - 1, turf);
            }
            //If the new turf allows atmospheric flow then add it to the processing queue
            else
            {
                //Check if we can bypass creating a new atmospheric block (See if surrounding turfs all have the same atmosphere)
                AtmosphericBlock newBlock = FindMergeBlock(createdTurf);

                //Perform appropriate action
                if (newBlock == null)
                    processingDeltas.Add(createdTurf.X, createdTurf.Y, createdTurf);
                else
                    newBlock.AddTurf(createdTurf);
            }
        }

        /// <summary>
        /// Returns the common atmosphere of a block.
        /// Returns null if there is no common atmosphere around a block, or no atmospheres at all
        /// </summary>
        private AtmosphericBlock FindMergeBlock(Turf createdTurf)
        {
            //Get surrounding turfs
            Turf above = World.GetTurf(createdTurf.X, createdTurf.Y + 1);
            Turf right = World.GetTurf(createdTurf.X + 1, createdTurf.Y);
            Turf below = World.GetTurf(createdTurf.X, createdTurf.Y - 1);
            Turf left  = World.GetTurf(createdTurf.X - 1, createdTurf.Y);
            //Check invalid
            //If any turf is null, then atmos will be null
            if (above == null || right == null || below == null || left == null)
                return null;
            //If there is a null atmosphere around us, then we return null
            AtmosphericBlock locatedCommonAtmosphere = above.AllowAtmosphericFlow ? above.Atmosphere
                : right.AllowAtmosphericFlow ? right.Atmosphere
                : below.AllowAtmosphericFlow ? below.Atmosphere
                : left.AllowAtmosphericFlow ? left.Atmosphere
                : null;
            //No atmospheres located around us
            if (locatedCommonAtmosphere == null)
                return null;
            //Check for validity (If there are multiple atmos groups, return null)
            if (above.AllowAtmosphericFlow && above.Atmosphere != locatedCommonAtmosphere)
                return null;
            if (right.AllowAtmosphericFlow && right.Atmosphere != locatedCommonAtmosphere)
                return null;
            if (below.AllowAtmosphericFlow && below.Atmosphere != locatedCommonAtmosphere)
                return null;
            if (left.AllowAtmosphericFlow && left.Atmosphere != locatedCommonAtmosphere)
                return null;
            //Return common atmosphere
            Log.WriteLine("Common atmosphere located");
            return locatedCommonAtmosphere;
        }

        /// <summary>
        /// Trigger a room check.
        /// This is for when a wall gets created and we need to check if a room is now sealed.
        /// </summary>
        private void ProcessDelta(int x, int y, Turf processing)
        {
            if (!processing.AllowAtmosphericFlow)
                throw new System.Exception($"A turf that doesn't allow atmospheric flow was somehow added to the process delta at {x}, {y}");
            //Atmosphere we are inheriting from
            AtmosphericBlock inherittedAtmosphere = processing.Atmosphere;
            //If the turf already has an atmosphere, use it, otherwise 
            AtmosphericBlock processingBlockAtmosphere = new AtmosphericBlock(processing);
            //A list of all the tiles we have searched already
            PositionBasedBinaryList<bool> searchedTiles = new PositionBasedBinaryList<bool>();
            //A list of all the positions which are at the edge of our search radius
            Queue<Vector<int>> searchRadiusEdgeTiles = new Queue<Vector<int>>();
            //Add the initial point
            searchRadiusEdgeTiles.Enqueue(new Vector<int>(x, y));
            searchedTiles.Add(x, y, true);
            //Begin expanding the area until something happens
            while (searchRadiusEdgeTiles.Count > 0)
            {
                //Get the tile we should search
                Vector<int> searchTile = searchRadiusEdgeTiles.Dequeue();
                //Check this tile
                Turf locatedTurf = World.GetTurf(searchTile[0], searchTile[1]);
                //If space, then we lose all of our gas and atmosphere :(
                if (locatedTurf == null)
                {
                    processingBlockAtmosphere.MergeAtmosphericBlockInto(null, searchTile);
                    return;
                }
                //If this turf was in the processing delta, consider it processed
                processingDeltas.Remove(searchTile[0], searchTile[1]);
                //If the located turf has no atmosphere, add it to our atmosphere
                if (locatedTurf.Atmosphere == null || locatedTurf.Atmosphere.Outdated)
                {
                    processingBlockAtmosphere.AddTurf(locatedTurf);
                }
                //If we find a turf with a different atmospheric block, then merge our atmosphere with their atmosphere.
                else if (locatedTurf.Atmosphere != processingBlockAtmosphere)
                {
                    processingBlockAtmosphere.MergeAtmosphericBlockInto(locatedTurf.Atmosphere, searchTile);
                }
                //Add surrounding tiles that allow gas flow
                //Check north
                if (World.AllowsAtmosphericFlow(searchTile[0], searchTile[1] + 1) && !searchedTiles.Get(searchTile[0], searchTile[1] + 1))
                {
                    searchedTiles.Add(searchTile[0], searchTile[1] + 1, true);
                    searchRadiusEdgeTiles.Enqueue(new Vector<int>(searchTile[0], searchTile[1] + 1));
                }
                //Check east
                if (World.AllowsAtmosphericFlow(searchTile[0] + 1, searchTile[1]) && !searchedTiles.Get(searchTile[0] + 1, searchTile[1]))
                {
                    searchedTiles.Add(searchTile[0] + 1, searchTile[1], true);
                    searchRadiusEdgeTiles.Enqueue(new Vector<int>(searchTile[0] + 1, searchTile[1]));
                }
                //Check south
                if (World.AllowsAtmosphericFlow(searchTile[0], searchTile[1] - 1) && !searchedTiles.Get(searchTile[0], searchTile[1] - 1))
                {
                    searchedTiles.Add(searchTile[0], searchTile[1] - 1, true);
                    searchRadiusEdgeTiles.Enqueue(new Vector<int>(searchTile[0], searchTile[1] - 1));
                }
                //Check west
                if (World.AllowsAtmosphericFlow(searchTile[0] - 1, searchTile[1]) && !searchedTiles.Get(searchTile[0] - 1, searchTile[1]))
                {
                    searchedTiles.Add(searchTile[0] - 1, searchTile[1], true);
                    searchRadiusEdgeTiles.Enqueue(new Vector<int>(searchTile[0] - 1, searchTile[1]));
                }
            }
            //Setup the atmosphere's gas
            if (inherittedAtmosphere != null)
            {
                //Calculate the proportion of gas to remove
                float proportion = Math.Min(processingBlockAtmosphere.ContainedAtmosphere.LitreVolume / inherittedAtmosphere.ContainedAtmosphere.LitreVolume, 1.0f);
                //Take some gas from the old atmosphere
                processingBlockAtmosphere.ContainedAtmosphere.InheritGasProportion(inherittedAtmosphere.ContainedAtmosphere, proportion);
                //Log data
                Log.WriteLine($"Inherited {proportion * 100}% of the atmosphere from block {inherittedAtmosphere.BlockId} (Outdated: {inherittedAtmosphere.Outdated})");
                Log.WriteLine($"Moles: {processingBlockAtmosphere.ContainedAtmosphere.Moles}, Pressure: {processingBlockAtmosphere.ContainedAtmosphere.KiloPascalPressure}, Temp: {processingBlockAtmosphere.ContainedAtmosphere.KelvinTemperature}");
            }
        }

        public override void InitSystem()
        {
            return;
        }

        protected override void AfterWorldInit()
        {
            return;
        }
    }
}
