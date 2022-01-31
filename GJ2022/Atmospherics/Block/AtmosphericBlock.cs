using GJ2022.Entities.Turfs;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Atmospherics.Block
{
    //A region which has shared atmosphere
    public class AtmosphericBlock
    {

        //The atmosphere we contain
        public Atmosphere ContainedAtmosphere { get; }

        //List of turfs that are in our atmospheric block
        private List<Turf> containedTurfs = new List<Turf>();

        /// <summary>
        /// When a wall or something is destroyed, the 2 atmos blocks on either side
        /// will merge and equalize their atmosphere
        /// Merge point indicates the point at which they are merging, gas will flow towards this point
        /// </summary>
        public void MergeAtmosphericBlocks(AtmosphericBlock otherBlock, Vector<int> mergePoint)
        {
            //Merged with space, fully remove all turfs from this block and null their atmosphere
            if (otherBlock == null)
            {
                for (int i = containedTurfs.Count - 1; i >= 0; i--)
                {
                    containedTurfs[i].Atmosphere = null;
                    containedTurfs.RemoveAt(i);
                }
                return;
            }
        }

        public void AddTurf(Turf turf)
        {
            containedTurfs.Add(turf);
        }

        public void RemoveTurf(Turf turf)
        {
            containedTurfs.Remove(turf);
        }

    }
}
