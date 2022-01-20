using GJ2022.Rendering.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GJ2022.Rendering.Textures
{
    //TODO add the texture cache
    public static class TextureCache
    {

        private const string ICON_PATH = "Data/IconFiles/";

        /// <summary>
        /// The error icon state to display if things fail
        /// </summary>
        private const string ERROR_ICON_STATE = "error";

        /// <summary>
        /// A store of the loaded texture files
        /// </summary>
        private static Dictionary<string, Texture> TextureFileCache = new Dictionary<string, Texture>();

        /// <summary>
        /// A store of the loaded texture json data
        /// </summary>
        private static Dictionary<string, TextureJson> TextureJsons = new Dictionary<string, TextureJson>();

        /// <summary>
        /// A store of the loaded block textures
        /// </summary>
        private static Dictionary<string, BlockTexture> BlockTextureCache = new Dictionary<string, BlockTexture>();

        /// <summary>
        /// Has loading been completed?
        /// </summary>
        public static bool LoadingComplete { get; private set; } = false;

        /// <summary>
        /// Does the error state exist?
        /// </summary>
        public static bool ErrorStateExists { get => BlockTextureCache.ContainsKey(ERROR_ICON_STATE); }

        /// <summary>
        /// Test the error state
        /// </summary>
        /// <returns></returns>
        public static string GetErrorFile()
        {
            BlockTexture errTex = BlockTextureCache[ERROR_ICON_STATE];
            TextureJson topFace = errTex.Faces[CubeFaceFlags.FACE_ABOVE];
            return $"{ICON_PATH}{topFace.FileName}";
        }

        /// <summary>
        /// Gets the texture uint of a loaded texture
        /// </summary>
        public static RendererTextureData GetTexture(string blockTexture, CubeFaceFlags faceFlag, bool checkSanity = false)
        {
            BlockTexture usingTexture;
            // Check if the block texture exists
            if (BlockTextureCache.ContainsKey(blockTexture))
                usingTexture = BlockTextureCache[blockTexture];
            else
            {
                Log.WriteLine($"Error, block texture: {blockTexture} not found!", LogType.WARNING);
                usingTexture = BlockTextureCache[ERROR_ICON_STATE];
            }
            // Find the files that we need to load
            TextureJson usingJson = usingTexture.Faces[faceFlag];
            //Locate the texture object we need
            if (TextureFileCache.ContainsKey(usingJson.FileName))
                return new RendererTextureData(TextureFileCache[usingJson.FileName], usingJson);
            else
            {
                try
                {
                    //Load the texture
                    TextureBitmap loadedBitmap = new TextureBitmap();
                    loadedBitmap.ReadTexture($"{ICON_PATH}{usingJson.FileName}");
                    //Cache the created texture
                    TextureFileCache.Add(usingJson.FileName, loadedBitmap);
                    //Return the created texture
                    return new RendererTextureData(loadedBitmap, usingJson);
                }
                catch (Exception e)
                {
                    //Catch whatever error we got (Probably lack of file)
                    Log.WriteLine(e, LogType.ERROR);
                    if (!checkSanity)
                    {
                        //Return a standard error texture
                        return GetTexture(ERROR_ICON_STATE, faceFlag, true);
                    }
                    else
                    {
                        //Just die at this point, our error icon doesn't exist.
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// Load the texture data json
        /// </summary>
        public static void LoadTextureDataJson()
        {
            //Start the texture loader thread
            new Thread(() => LoadTextureDataJsonThread()).Start();
        }

        /// <summary>
        /// Seperate thread for loading the texture Json
        /// </summary>
        private static void LoadTextureDataJsonThread(bool catchErrors = true)
        {
            //Loaded texture data
            Log.WriteLine("Loading texture data...", LogType.MESSAGE);
            //Start loading and parsing the data
            JObject loadedJson = JObject.Parse(File.ReadAllText("Data/TextureData.json"));
            //Json file loaded
            Log.WriteLine("Json file loaded and parsed, populating block texture cache", LogType.MESSAGE);
            //Load the texture jsons
            JToken texturesProperty = loadedJson["textures"];
            foreach (JToken value in texturesProperty)
            {
                try
                {
                    string id = value.Value<string>("id");
                    string file = value.Value<string>("file");
                    int width = value.Value<int>("width");
                    int height = value.Value<int>("height");
                    int index_x = value.Value<int>("index_x");
                    int index_y = value.Value<int>("index_y");
                    //Create the texture json
                    TextureJson createdJson = new TextureJson(file, width, height, index_x, index_y);
                    //Cache it
                    TextureJsons.Add(id, createdJson);
                }
                catch (Exception e)
                {
                    if (catchErrors)
                    {
                        //TODO: Error handling
                        Log.WriteLine(e, LogType.ERROR);
                    }
                    else
                    {
                        Log.WriteLine(e.StackTrace);
                        LoadingComplete = true;
                        throw e;
                    }
                }
            }
            //Loaded Texture cache
            Log.WriteLine($"Successfully loaded data about {TextureJsons.Count} textures.", LogType.MESSAGE);
            //Load the block textures
            JToken blockTextureProperty = loadedJson["blockTextures"];
            foreach (JToken value in blockTextureProperty)
            {
                try
                {
                    //Load the block texture name
                    string block = value.Value<string>("block");
                    JObject faces = value.Value<JObject>("faces");
                    //Load the face data
                    string top = faces.Value<string>("top");
                    string bottom = faces.Value<string>("bottom");
                    string left = faces.Value<string>("left");
                    string right = faces.Value<string>("right");
                    string back = faces.Value<string>("back");
                    string front = faces.Value<string>("front");
                    //Create the block texture data
                    BlockTextureCache.Add(
                        block,
                        new BlockTexture(
                            TextureJsons[top],
                            TextureJsons[bottom],
                            TextureJsons[left],
                            TextureJsons[right],
                            TextureJsons[back],
                            TextureJsons[front]
                        )
                    );
                }
                catch (Exception e)
                {
                    if (catchErrors)
                    {
                        //TODO: Error handling
                        Log.WriteLine(e, LogType.ERROR);
                    }
                    else
                    {
                        LoadingComplete = true;
                        throw e;
                    }
                }
            }
            //load the block textures
            Log.WriteLine($"Loaded {BlockTextureCache.Count} block textures", LogType.MESSAGE);
            //All texture data loaded
            Log.WriteLine("All texture data loaded!", LogType.MESSAGE);
            //Loading completed
            LoadingComplete = true;
        }

    }
}
