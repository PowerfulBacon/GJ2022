using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GJ2022.EntityLoading
{
    public static class EntityLoader
    {

        /// <summary>
        /// Load the entity XML file.
        /// </summary>
        public static void LoadEntities()
        {
            
        }

        private static void LoadEntityFile(string file)
        {
            XmlDocument entityDocument = new XmlDocument();
            entityDocument.Load(file);
        }

    }
}
