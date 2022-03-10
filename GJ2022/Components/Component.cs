using GJ2022.EntityLoading;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components
{
    public abstract class Component : IInstantiatable
    {

        public ComponentHandler Parent { get; private set; }

        /// <summary>
        /// Create the component and attach it to a parent
        /// </summary>
        public void Attach(ComponentHandler parent)
        {
            Parent = parent;
        }

        public void Initialize(Vector<float> initializePosition)
        { }

        /// <summary>
        /// Called after a component has been added to an entity
        /// and all the properties have been setup.
        /// </summary>
        public abstract void OnComponentAdd();

        /// <summary>
        /// Called when the component is removed from the parent
        /// </summary>
        public abstract void OnComponentRemove();

        public void PreInitialize(Vector<float> initializePosition)
        { }

        public abstract void SetProperty(string name, object property);

    }
}
