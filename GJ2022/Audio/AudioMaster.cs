using Silk.NET.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Audio
{
    public unsafe static class AudioMaster
    {

        private static Device* audioDevice = null;
        private static Context* audioContext = null;

        private static List<uint> buffers = new List<uint>();

        /// <summary>
        /// Initialize the audio master.
        /// Will cleanup the old devices and contexts if called multiple times.
        /// </summary>
        public static bool Initialize()
        {
            //Get the AL context
            ALContext audioLibraryContext = ALContext.GetApi();

            //Delete existing audio device
            if (audioDevice != null)
            {
                audioLibraryContext.CloseDevice(audioDevice);
                audioDevice = null;
            }

            //Delete existing audio context
            if(audioContext != null)
            {
                audioLibraryContext.DestroyContext(audioContext);
                audioContext = null;
            }

            //Setup the audio device
            audioDevice = audioLibraryContext.OpenDevice("");

            //Check to see if audio device creation was successful
            if (audioDevice == null)
            {
                Log.WriteLine("Error: Unable to create OpenAL audio device!", LogType.ERROR);
                return false;
            }

            //Create audio context
            audioContext = audioLibraryContext.CreateContext(audioDevice, null);
            audioLibraryContext.MakeContextCurrent(audioContext);

            //Get errors
            AL audioLib = AL.GetApi();
            AudioError error = audioLib.GetError();
            if (error != AudioError.NoError)
                throw new Exception($"Audio error: {error}");

            //Setup was successful
            return true;
        }

        /// <summary>
        /// Cleanup the audio files
        /// </summary>
        public static void Cleanup()
        {
            ALContext audioLibraryContext = ALContext.GetApi();
            AL audioLibrary = AL.GetApi();
            audioLibraryContext.CloseDevice(audioDevice);
            audioLibraryContext.DestroyContext(audioContext);
            audioLibraryContext.Dispose();
            audioLibrary.Dispose();
        }

    }
}
