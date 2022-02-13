using GJ2022.Entities.Structures.Power;
using GJ2022.Game;
using GJ2022.Game.Power;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.ComponentInterfaces
{
    /// <summary>
    /// Represents something that is directly connected to a cable in the powergrid
    /// </summary>
    public interface IPowerGridConnected
    {

        /// <summary>
        /// The thing that interacts with the powernet
        /// </summary>
        PowernetInteractor PowernetInteractor { get; }

    }
}
