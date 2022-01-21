using GLFW;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static OpenGL.Gl;

namespace GJ2022.Tests.RendererTests.ShaderTests
{
    [TestClass]
    [DeploymentItem(@".\glfw.dll")]
    [DeploymentItem(@".\GLFW.NET.dll")]
    [DeploymentItem(@".\Rendering\Shaders\instanceShader.vert")]
    [DeploymentItem(@".\Rendering\Shaders\instanceShader.frag")]
    [DeploymentItem(@".\Rendering\Shaders\backgroundShader.vert")]
    [DeploymentItem(@".\Rendering\Shaders\backgroundShader.frag")]
    [DeploymentItem(@".\Rendering\Shaders\outlineShader.vert")]
    [DeploymentItem(@".\Rendering\Shaders\outlineShader.frag")]
    public class ShaderTests
    {

        bool setupFailed = false;

        uint programUint;

        public ShaderTests()
        {
            try
            {
                Glfw.WindowHint(Hint.ContextVersionMajor, 3);
                Glfw.WindowHint(Hint.ContextVersionMinor, 3);
                Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
                Window window = Glfw.CreateWindow(1, 1, "UNIT TEST WINDOW", GLFW.Monitor.None, Window.None);
                Glfw.MakeContextCurrent(window);
                Import(Glfw.GetProcAddress);
                //Create the program
                programUint = glCreateProgram();
            }
            catch (GLFW.Exception)
            {
                setupFailed = true;
            }
        }

        ~ShaderTests()
        {
            if (!setupFailed)
            {
                glDeleteProgram(programUint);
                //Terminate GLFW
                Glfw.Terminate();
            }
        }

        [TestMethod]
        public void TestShaderExists()
        {
            //Sanity checks
            //Tests the functionality of these tests
            Assert.IsTrue(File.Exists(@"instanceShader.vert"), "Couldn't locate instance shader file, it is likely this test is incorrectly setup.");
        }

        [TestMethod]
        public void TestOutlineShaderCompilation()
        {
            if (setupFailed) Assert.Inconclusive();
            string name = "outlineShader";
            Assert.IsTrue(TestShader(name, GL_FRAGMENT_SHADER));
            Assert.IsTrue(TestShader(name, GL_VERTEX_SHADER));
        }

        [TestMethod]
        public void TestBackgroundFragmentShaderCompilation()
        {
            if (setupFailed) Assert.Inconclusive();
            string name = "backgroundShader";
            Assert.IsTrue(TestShader(name, GL_FRAGMENT_SHADER));
        }

        [TestMethod]
        public void TestBackgroundVertexShaderCompilation()
        {
            if (setupFailed) Assert.Inconclusive();
            string name = "backgroundShader";
            Assert.IsTrue(TestShader(name, GL_VERTEX_SHADER));
        }

        [TestMethod]
        public void TestFragmentShaderCompilation()
        {
            if (setupFailed) Assert.Inconclusive();
            string name = "instanceShader";
            Assert.IsTrue(TestShader(name, GL_FRAGMENT_SHADER));
        }

        [TestMethod]
        public void TestVertexShaderCompilation()
        {
            if (setupFailed) Assert.Inconclusive();
            string name = "instanceShader";
            Assert.IsTrue(TestShader(name, GL_VERTEX_SHADER));
        }

        private bool TestShader(string shaderName, int type)
        {
            string extension = type == GL_VERTEX_SHADER ? "vert" : "frag";
            //Generate the shaders
            uint shader = glCreateShader(type);
            //Load and compile shaders
            //Read the shader sources
            glShaderSource(shader, File.ReadAllText($"{shaderName}.{extension}"));
            //Compile the shaders
            glCompileShader(shader);
            string shaderLog = glGetShaderInfoLog(shader);
            //Print the result of compiling the shader
            Log.WriteLine($"{shaderName}.{extension}: {shaderLog}");
            //Print a debug message
            Log.WriteLine($"Loaded {shaderName} shaders successfully.");
            return shaderLog == string.Empty;
        }

    }
}
