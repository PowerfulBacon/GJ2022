using Silk.NET.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Audio
{
    public class AudioSource
    {

        private uint source;

        public void PlaySound(string file, int x, int y)
        {
            //Get the AL API
            AL al = AL.GetApi();
            //Get the audio file
            AudioFile audioFile = AudioCache.GetAudioFile(file);
            //Create the audio source
            source = al.GenSource();
            //Play the source
            al.SetSourceProperty(source, SourceInteger.Buffer, audioFile.Buffer);
            al.SourcePlay(source);
        }

    }
}
