using GJ2022.Rendering.Textures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace GJ2022.Tests.RendererTests.TextureTests
{
    [TestClass]
    [DeploymentItem(@".\Data\", @".\Data")]
    public class TestJsonTexture
    {

        [TestMethod]
        public void TestJsonParsing()
        {
            MethodInfo loadTextureJson = typeof(TextureCache).GetMethod("LoadTextureDataJsonThread", BindingFlags.Static | BindingFlags.NonPublic);
            loadTextureJson.Invoke(null, new object[1] { false });
            Assert.IsTrue(TextureCache.ErrorStateExists, "The error icon state does not exist!");
            Assert.IsTrue(File.Exists(TextureCache.GetErrorFile()), "The error texture file doesn't appear to exist");
        }

    }
}
