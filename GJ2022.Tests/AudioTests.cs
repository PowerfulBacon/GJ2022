using GJ2022.Audio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Tests
{
    [TestClass]
    [DeploymentItem(@".\Silk.NET.Core.dll")]
    [DeploymentItem(@".\Silk.NET.OpenAL.dll")]
    [DeploymentItem(@".\Data\Audio\")]
    public class AudioTests
    {

        [TestMethod]
        public void TestAudioFiles()
        {
            //Initialize the audio master
            AudioMaster.Initialize();
            //Check each file
            foreach (string fileName in Directory.GetFiles(@".\", "*.wav", SearchOption.AllDirectories))
            {
                //Check this
                new AudioFile(fileName);
                //Sound check successful
                Log.WriteLine($"Compiled {fileName} successfully.");
            }
            //Cleanup the audio master
            AudioMaster.Cleanup();
        }

    }
}
