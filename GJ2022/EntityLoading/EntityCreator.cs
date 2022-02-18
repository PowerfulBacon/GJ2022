using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.EntityLoading
{
    public static class EntityCreator
    {

        public static void CreateEntity(string name, Vector<float> position)
        {
            EntityConfig.LoadedEntityDefs[name].InstantiateAt(position);
        }

        /// <summary>
        /// Create an entity from the xml construct at a specified position.
        /// </summary>
        /// <param name="name">The name of the entity's xml data.</param>
        /// <param name="position">The position to spawn the entity at.</param>
        public static T CreateEntity<T>(string name, Vector<float> position)
        {
            return (T)EntityConfig.LoadedEntityDefs[name].InstantiateAt(position);
        }

    }
}
