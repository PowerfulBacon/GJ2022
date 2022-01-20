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
        public void TestFragmentShaderCompilation()
        {
            if (setupFailed) Assert.Inconclusive();
            string name = "instanceShader";
            //Generate the shaders
            uint shader = glCreateShader(GL_FRAGMENT_SHADER);
            //Load and compile shaders
            //Read the shader sources
            glShaderSource(shader, File.ReadAllText($"{name}.frag"));
            //Compile the shaders
            glCompileShader(shader);
            //Print the result of compiling the shader
            Log.WriteLine($"{name}.vert: {glGetShaderInfoLog(shader)}");
            //Print a debug message
            Log.WriteLine($"Loaded {name} shaders successfully.");
        }

        [TestMethod]
        public void TestVertexShaderCompilation()
        {
            if (setupFailed) Assert.Inconclusive();
            string name = "instanceShader";
            //Generate the shaders
            uint shader = glCreateShader(GL_VERTEX_SHADER);
            //Load and compile shaders
            //Read the shader sources
            glShaderSource(shader, File.ReadAllText($"{name}.vert"));
            //Compile the shaders
            glCompileShader(shader);
            //Print the result of compiling the shader
            Log.WriteLine($"{name}.vert: {glGetShaderInfoLog(shader)}");
            //Print a debug message
            Log.WriteLine($"Loaded {name} shaders successfully.");
        }

    }
}
