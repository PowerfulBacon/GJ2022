using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.EntityLoading
{
    public interface IInstantiatable
    {

        /// <summary>
        /// Set the property of this object
        /// </summary>
        void SetProperty(string name, object property);

        /// <summary>
        /// Called after the object has been instantiated and all the properties have been set.
        /// </summary>
        void Initialize(Vector<float> initializePosition);

        void PreInitialize(Vector<float> initializePosition);

    }
}
