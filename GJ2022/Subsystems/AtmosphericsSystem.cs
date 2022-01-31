using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_PROCESSING;

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
                block.MergeAtmosphericBlocks(null, new Vector<int>(destroyedTurf.X, destroyedTurf.Y));
        }

        //Turf => Turf
        public void OnTurfChanged(Turf oldTurf, Turf newTurf)
        {
            //Atmosflow allowed => allowed - inherit the old turfs atmosphere
            //Atmosflow allowed => disallowed - Check to see if we need to split the atmosphere area in half (Add surrounding tiles to processing queue)
            //Atmosflow disallowed => allowed - Merge surrounding atmospheres
            //Atmosflow disallowed => disallowed - Do nothing
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
                processingDeltas.Add(createdTurf.X, createdTurf.Y, createdTurf);
            }
        }

        /// <summary>
        /// Trigger a room check.
        /// This is for when a wall gets created and we need to check if a room is now sealed.
        /// </summary>
        private void ProcessDelta(int x, int y, Turf processing)
        {
            //If the turf already has an atmosphere, use it, otherwise 
            AtmosphericBlock processingBlockAtmosphere = processing.Atmosphere ?? new AtmosphericBlock(processing);
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
                    processingBlockAtmosphere.MergeAtmosphericBlocks(null, searchTile);
                    return;
                }
                //If this turf was in the processing delta, consider it processed
                processingDeltas.Remove(searchTile[0], searchTile[1]);
                //If the located turf has no atmosphere, add it to our atmosphere
                if (locatedTurf.Atmosphere == null)
                {
                    processingBlockAtmosphere.AddTurf(locatedTurf);
                }
                //If we find a turf with a different atmospheric block, then merge our atmosphere with their atmosphere.
                else if (locatedTurf.Atmosphere != processingBlockAtmosphere)
                {
                    processingBlockAtmosphere.MergeAtmosphericBlocks(locatedTurf.Atmosphere, searchTile);
                    return;
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
            //Delta processing completed
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
