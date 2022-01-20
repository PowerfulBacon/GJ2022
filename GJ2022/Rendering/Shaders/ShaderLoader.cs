using System;

namespace GJ2022.Rendering.Shaders
{
    [Obsolete("Gunna be depreciated and removed")]
    public class ShaderLoader
    {

        //Relative directory of the shaders to the .exe
        private const string SHADER_DIRECTORY = "./Rendering/Shaders/";

        //The shader set.
        public static ShaderSet shaders_simple;

        /// <summary>
        /// Generates the shaders and attaches them to the program
        /// </summary>
        /// <param name="program">The uint of the program.</param>
        public static void GenerateShaders()
        {
            //Load the 'simple' shader set.
            //Simple is actually not that simple anymore since it has been expanded on a lot.
            shaders_simple = new ShaderSet("simple", SHADER_DIRECTORY);
        }

        public static void AttachShaders(uint program)
        {
            shaders_simple.AttachShaders(program);
        }

        public static void DeleteShaders()
        {
            shaders_simple.DeleteShaders();
        }

    }
}
