using GJ2022.EntityLoading.XmlDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.EntityLoading
{
    public static class EntityConfig
    {

        /// <summary>
        /// Dictionary containing all loaded entity defs.
        /// </summary>
        public static Dictionary<string, EntityDef> LoadedEntityDefs = new Dictionary<string, EntityDef>();

        /// <summary>
        /// A cache of all classes
        /// </summary>
        public static Dictionary<string, Type> ClassTypeCache = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IInstantiatable).IsAssignableFrom(type))
            .GroupBy(type => type.Name, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(typeGroup => typeGroup.Key, typeGroup => typeGroup.Last(), StringComparer.OrdinalIgnoreCase);

    }
}
