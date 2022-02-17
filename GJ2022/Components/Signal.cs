using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components
{
    public enum Signal
    {
        SIGNAL_ENTITY_MOVED,
        SIGNAL_ENTITY_DESTROYED,
        //Gas Storage Signals
        SIGNAL_GET_ATMOSPHERE,      //! Return the atmospheric contents
    }
}
