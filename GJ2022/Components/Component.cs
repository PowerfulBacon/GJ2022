using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components
{
    public abstract class Component
    {

        /// <summary>
        /// Called after a component has been added to an entity
        /// and all the properties have been setup.
        /// </summary>
        public abstract void OnComponentAdd();

        /// <summary>
        /// Called when the component is removed from the parent
        /// </summary>
        public abstract void OnComponentRemove();

    }
}
