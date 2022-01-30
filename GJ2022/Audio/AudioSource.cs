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

        public void PlaySound(string file, float x, float y, float z = 0, float gain = 1.0f, bool repeating = false)
        {
            //Get the AL API
            AL al = AL.GetApi();
            //Get the audio file
            AudioFile audioFile = AudioCache.GetAudioFile(file);
            //Create the audio source
            source = al.GenSource();
            //Play the source
            if(gain != 1.0f)
                al.SetSourceProperty(source, SourceFloat.Gain, gain);
            if (repeating)
                al.SetSourceProperty(source, SourceBoolean.Looping, repeating);
            al.SetSourceProperty(source, SourceInteger.Buffer, audioFile.Buffer);
            al.SetSourceProperty(source, SourceVector3.Position, x, y, z);
            al.SourcePlay(source);
            //Delete the source after the playtime
            if (!repeating)
            {
                Task.Run(async delegate
                {
                    await Task.Delay((int)(audioFile.PlayTime * 1000) + 1000);
                    al.DeleteSource(source);
                });
            }
        }

    }
}
