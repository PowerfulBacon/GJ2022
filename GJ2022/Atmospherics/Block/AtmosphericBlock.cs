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

        private static int AtmosphericBlocks = 0;

        //The atmosphere we contain
        public Atmosphere ContainedAtmosphere { get; }

        //List of turfs that are in our atmospheric block
        private List<Turf> containedTurfs = new List<Turf>();

        public int BlockId { get; } = AtmosphericBlocks++;

        //If outdated, the atmos system won't merge into this
        public bool Outdated { get; set; } = false;

        public AtmosphericBlock(Turf parent)
        {
            //Setup the initial atmosphere
            ContainedAtmosphere = Atmosphere.SpaceAtmosphere;
            ContainedAtmosphere.AttachBlock(this);
            //Add the turf
            AddTurf(parent);
        }

        /// <summary>
        /// When a wall or something is destroyed, the 2 atmos blocks on either side
        /// will merge and equalize their atmosphere
        /// Merge point indicates the point at which they are merging, gas will flow towards this point
        /// </summary>
        public void MergeAtmosphericBlockInto(AtmosphericBlock otherBlock, Vector<int> mergePoint)
        {
            //Merged with space, fully remove all turfs from this block and null their atmosphere
            if (otherBlock == null)
            {
                //Pressure = force / area
                float spacePressureDelta = ContainedAtmosphere.KiloPascalPressure;
                //Remove the gas
                for (int i = containedTurfs.Count - 1; i >= 0; i--)
                {
                    containedTurfs[i].AtmosphericPressureChangeReact(mergePoint, spacePressureDelta);
                    containedTurfs[i].Atmosphere = null;
                    containedTurfs[i].OnAtmosphericBlockChanged(null);
                    containedTurfs.RemoveAt(i);
                }
                return;
            }
            //We merged with another atmos block, equalize pressure and calculate delta
            float pressureDelta = ContainedAtmosphere.KiloPascalPressure - otherBlock.ContainedAtmosphere.KiloPascalPressure;
            bool flowingOutwards = true;
            if (pressureDelta < 0)
            {
                flowingOutwards = false;
                pressureDelta = -pressureDelta;
            }
            //Merge the atmospheres
            ContainedAtmosphere.Merge(otherBlock.ContainedAtmosphere);
            //If we are flowing into them, blow our turfs
            //Do this before merging
            if (flowingOutwards)
            {
                for (int i = containedTurfs.Count - 1; i >= 0; i--)
                {
                    containedTurfs[i].AtmosphericPressureChangeReact(mergePoint, pressureDelta);
                }
            }
            //Merge the other turfs into ourself
            for (int i = otherBlock.containedTurfs.Count - 1; i >= 0; i--)
            {
                //Assimilate their turf. Do this directly rather than through addTurf, since Atmosphere.Merge handles volume changes.
                otherBlock.containedTurfs[i].Atmosphere = this;
                otherBlock.containedTurfs[i].OnAtmosphericBlockChanged(this);
                containedTurfs.Add(otherBlock.containedTurfs[i]);
                //If the pressure delta is less than 0, blow their turfs into us
                if(!flowingOutwards)
                    otherBlock.containedTurfs[i].AtmosphericPressureChangeReact(mergePoint, pressureDelta);
            }
        }

        public void ChangeTurf(Turf oldTurf, Turf newTurf)
        {
            if (newTurf.Destroyed)
                throw new Exception("Turf destroyed exception!");
            containedTurfs.Remove(oldTurf);
            containedTurfs.Add(newTurf);
            newTurf.Atmosphere = this;
            newTurf.OnAtmosphericBlockChanged(this);
        }

        /// <summary>
        /// Add a turf to our atmospheric block and adjust the volume to accomodate it.
        /// </summary>
        public void AddTurf(Turf turf)
        {
            if (turf.Destroyed)
                throw new Exception("Turf destroyed exception!");
            containedTurfs.Add(turf);
            ContainedAtmosphere.SetVolume(ContainedAtmosphere.LitreVolume + AtmosphericConstants.TILE_GAS_VOLUME);
            turf.Atmosphere = this;
            turf.OnAtmosphericBlockChanged(this);
        }

        /// <summary>
        /// Remove a turf from our atmospheric block and adjust the volume to accomodate it.
        /// This pushes the gas that was on that turf back into this, increasing the overall pressure and temperature.
        /// </summary>
        public void RemoveTurf(Turf turf)
        {
            containedTurfs.Remove(turf);
            ContainedAtmosphere.SetVolume(ContainedAtmosphere.LitreVolume - AtmosphericConstants.TILE_GAS_VOLUME);
            turf.Atmosphere = null;
            if(!turf.Destroyed)
                turf.OnAtmosphericBlockChanged(null);
        }

        public void UpdateGasTurfs()
        {
            foreach (Turf turf in containedTurfs)
            {
                turf.OnAtmopshereContentsChanged(this);
            }
        }

    }
}
