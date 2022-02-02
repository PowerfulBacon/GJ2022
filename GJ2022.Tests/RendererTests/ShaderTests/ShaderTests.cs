using GLFW;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static OpenGL.Gl;

namespace GJ2022.Tests.RendererTests.ShaderTests
{
    [TestClass]
    [DeploymentItem(@".\glfw.dll")]
    [DeploymentItem(@".\GLFW.NET.dll")]
    [DeploymentItem(@".\Rendering\Shaders\")]
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
            catch (Exception e)
            {
                setupFailed = true;
                Log.WriteLine(e, LogType.ERROR);
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
        public void TestShaderCompilation()
        {
            if (setupFailed) Assert.Inconclusive();
            foreach (string fileName in Directory.GetFiles(@".\", "*.vert", SearchOption.AllDirectories))
            {
                string sanitizedName = fileName.Replace(".vert", "");
                Assert.IsTrue(TestShader(sanitizedName, GL_FRAGMENT_SHADER), $"{sanitizedName}.frag failed to compile!");
                Assert.IsTrue(TestShader(sanitizedName, GL_VERTEX_SHADER), $"{sanitizedName}.vert failed to compile!");
                Log.WriteLine($"Compiled {sanitizedName} successfully.");
            }
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
