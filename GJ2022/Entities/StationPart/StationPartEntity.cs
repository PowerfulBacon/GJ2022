using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.StationPart
{
    abstract class StationPartEntity
    {

        //List of rooms connected to this one
        public List<StationPartEntity> AttachedRooms { get; } = new List<StationPartEntity>();

        //Get the connection points of the station part.
        public abstract Vector[] GetConnectionPoints();

    }
}
