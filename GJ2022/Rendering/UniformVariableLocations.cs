using static OpenGL.Gl;

namespace GJ2022.Rendering
{
    /// <summary>
    /// Getting uniform variable locations is quite slow.
    /// Luckily they don't change, meaning we only have to load them once!
    /// This makes the renderer much faster.
    /// </summary>
    public class UniformVariableLocations
    {

        //Uniform location of the view matrix shader variable
        public int viewMatrixUniformLocation { get; private set; }

        //Uniform location of the projection matrix shader variable
        public int projectionMatrixUniformLocation { get; private set; }

        //Uniform location of the object matrix
        public int objectMatrixUniformLocation { get; private set; }

        public void LoadUniformLocations(uint programUint)
        {
            viewMatrixUniformLocation = glGetUniformLocation(programUint, "viewMatrix");
            projectionMatrixUniformLocation = glGetUniformLocation(programUint, "projectionMatrix");
            //objectMatrixUniformLocation = glGetUniformLocation(programUint, "objectMatrix");
        }

    }
}
