using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GJ2022.EntityLoading
{
    public static class EntityLoader
    {

        private const string GAME_DATA_DIRECTORY = "Data/GameData";

        /// <summary>
        /// Load the entity XML file.
        /// </summary>
        public static void LoadEntities()
        {
            //Clear existing data
            if (EntityConfig.LoadedEntityDefs.Count > 0)
            {
                Log.WriteLine("Clearing existing EntityConfig data... (LoadEntities was called again, likely a debug call)", LogType.WARNING);
                EntityConfig.LoadedEntityDefs.Clear();
            }
            //Locate all game data XML files.
            foreach (string gameDataGroupName in Directory.GetDirectories(GAME_DATA_DIRECTORY))
            {
                //Locate the XML files at this location
                string[] locatedFiles = Directory.GetFiles(gameDataGroupName, "*.xml", SearchOption.AllDirectories);
                //Log the amount of files we are loading
                Log.WriteLine($"Loading {gameDataGroupName} ({locatedFiles.Length} files located)", LogType.LOG);
                //Load the files
                foreach (string xmlFile in locatedFiles)
                {
                    LoadEntityFile(xmlFile);
                }
            }
            //Create a log message
            Log.WriteLine($"Loaded {EntityConfig.LoadedEntityDefs.Count} entities...", LogType.LOG);
        }

        private static void LoadEntityFile(string file)
        {
            //Load the XML file
            XElement entitiesElement = XElement.Load(file);
            //TODO
            throw new NotImplementedException();
        }

    }
}
