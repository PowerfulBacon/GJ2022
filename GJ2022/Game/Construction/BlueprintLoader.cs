using GJ2022.Entities;
using GJ2022.Entities.Turfs;
using GJ2022.Game.Construction.Blueprints;
using GJ2022.Game.Construction.BlueprintSets;
using GJ2022.Game.Construction.Cost;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GJ2022.Game.Construction
{
    public static class BlueprintLoader
    {

        public static bool BlueprintsLoaded = false;

        public static Dictionary<string, BlueprintCategory> BlueprintCategories = new Dictionary<string, BlueprintCategory>();

        public static void LoadBlueprints()
        {
            new Task(() => _LoadBlueprints()).Start();
        }

        private static void _LoadBlueprints()
        {
            try
            {
                //Load all possible types
                Dictionary<string, Type> possibleTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type => type.IsSubclassOf(typeof(Entity)) || type.IsSubclassOf(typeof(Turf)))
                    .GroupBy(type => type.Name, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(typeGroup => typeGroup.Key, typeGroup => typeGroup.Last(), StringComparer.OrdinalIgnoreCase);

                //Loaded texture data
                Log.WriteLine("Loading blueprint data...", LogType.MESSAGE);
                //Start loading and parsing the data
                JObject loadedJson = JObject.Parse(File.ReadAllText("Data/ConstructionData/BlueprintData.json"));
                //Json file loaded
                Log.WriteLine("Blueprint json file loaded and parsed", LogType.MESSAGE);
                //==========================
                //==========================
                // Load blueprints
                //==========================
                JToken blueprintTokens = loadedJson["blueprints"];
                Dictionary<string, BlueprintDetail> loadedBlueprintDictionary = new Dictionary<string, BlueprintDetail>();
                foreach (JToken blueprintToken in blueprintTokens)
                {
                    try
                    {
                        //Load the blueprint type
                        Type blueprintType = possibleTypes[blueprintToken.Value<string>("blueprintType")];
                        //Load the created type
                        Type createdType = possibleTypes[blueprintToken.Value<string>("created_type")];
                        //Parse the cost data
                        ConstructionCostData costData = new ConstructionCostData();
                        //Get each cost
                        foreach (JProperty costToken in blueprintToken["cost"])
                        {
                            costData.Cost.Add(possibleTypes[costToken.Name], costToken.Value.Value<int>());
                        }
                        //Load the blueprint texture
                        string texture = blueprintToken.Value<string>("texture");
                        //Load the blueprint layer
                        int layer = blueprintToken.Value<int>("layer");
                        //Load the priority
                        int priority = blueprintToken.Value<int>("priority");
                        BlueprintDetail blueprintDetail = new BlueprintDetail(blueprintType, costData, layer, texture, createdType, priority);
                        loadedBlueprintDictionary.Add(blueprintToken.Value<string>("id"), blueprintDetail);
                    }
                    catch (Exception e)
                    {
                        Log.WriteLine(e, LogType.ERROR);
                    }
                }
                Log.WriteLine($"Loaded {loadedBlueprintDictionary.Count} blueprint(s).");
                //==========================
                // Load Blueprint Sets
                // Depends on blueprints
                //==========================
                JToken blueprintSetTokens = loadedJson["blueprintSets"];
                Dictionary<string, BlueprintSet> loadedBlueprintSets = new Dictionary<string, BlueprintSet>();
                foreach (JToken blueprintSetToken in blueprintSetTokens)
                {
                    try
                    {
                        loadedBlueprintSets.Add(
                            blueprintSetToken.Value<string>("id"),
                            new BlueprintSet(
                                blueprintSetToken.Value<string>("name"),
                                blueprintSetToken.Value<bool>("isRoom"),
                                loadedBlueprintDictionary[blueprintSetToken.Value<string>("edge")],
                                loadedBlueprintDictionary[blueprintSetToken.Value<string>("fill")]
                                )
                            );
                    }
                    catch (Exception e)
                    {
                        Log.WriteLine(e, LogType.ERROR);
                    }
                }
                Log.WriteLine($"Loaded {loadedBlueprintSets.Count} blueprint set(s).");
                //==========================
                // Load blueprint categories
                // Depends on blueprint sets being loaded
                //==========================
                JToken blueprintCategories = loadedJson["blueprintCategories"];
                foreach (JToken blueprintCategory in blueprintCategories)
                {
                    try
                    {
                        BlueprintCategory createdCateogry = new BlueprintCategory(blueprintCategory.Value<string>("name"));
                        foreach (string categoryContent in blueprintCategory["contents"])
                        {
                            createdCateogry.Contents.Add(loadedBlueprintSets[categoryContent]);
                        }
                        BlueprintCategories.Add(createdCateogry.Name, createdCateogry);
                    }
                    catch (Exception e)
                    {
                        Log.WriteLine(e, LogType.ERROR);
                    }
                }
                Log.WriteLine($"Loaded {BlueprintCategories.Count} blueprint categorie(s).");
            }
            catch (Exception e)
            {
                Log.WriteLine("===FAILED TO LOAD BLUEPRINT DATA===", LogType.ERROR);
                Log.WriteLine(e, LogType.ERROR);
                Environment.Exit(1);
            }
            //==========================
            // Blueprints loaded successfully.
            //==========================
            BlueprintsLoaded = true;
        }

    }
}
