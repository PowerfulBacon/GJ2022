using GJ2022.Audio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

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
            try
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
            catch (FileNotFoundException fileNotFound)
            {
                if (fileNotFound.FileName == "openal32.dll")
                    Assert.Inconclusive("Unable to locate openal32.dll, cannot run tests.");
            }
        }

    }
}
