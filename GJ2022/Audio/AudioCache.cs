using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Audio
{
    public static class AudioCache
    {

        private const string AUDIO_PATH = "Data/Audio/";

        private static Dictionary<string, AudioFile> audioFileCache = new Dictionary<string, AudioFile>();

        public static AudioFile GetAudioFile(string file)
        {
            string filePath = $"{AUDIO_PATH}{file}";
            //Already cached
            if (audioFileCache.ContainsKey(file))
                return audioFileCache[file];
            //Load into the cache
            AudioFile createdFile = new AudioFile(filePath);
            audioFileCache.Add(file, createdFile);
            return createdFile;
        }

    }
}
