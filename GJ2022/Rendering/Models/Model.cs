using GLFW;
using static OpenGL.Gl;

namespace GJ2022.Rendering.Models
{
    public class Model
    {

        //Length of the model's vertices
        public int VerticesLength { get; private set; }

        //The vertex array object
        //Contains one or more vertex buffer objects and stores 
        //information about the complete rendered object.
        public uint VertexArrayObject { get; private set; }

        //The vertex buffer object
        //A memory buffer in the high speed memory of the GPU
        //which holds information about vertices.
        public uint VertexBufferObject { get; private set; }

        //The UV buffer
        //Similar to the VBO, except it is a memory buffer for
        //sending data about the UVs.
        public uint UvBuffer { get; private set; }

        /// <summary>
        /// Model with provided vertices and UVs.
        /// </summary>
        public Model(float[] vertices, float[] uvs)
        {
            if(Program.UsingOpenGL)
                GenerateBuffers(vertices, uvs);
        }

        /// <summary>
        /// Generates and populates the buffers that represent this
        /// model.
        /// The buffers are very fast ways to transfer the data to the
        /// GPU.
        /// </summary>
        private unsafe void GenerateBuffers(float[] vertices, float[] uvs)
        {
            //Length
            VerticesLength = vertices.Length;

            //Generate the VAO
            VertexArrayObject = glGenVertexArray();
            glBindVertexArray(VertexArrayObject);

            //Generate the VBO
            VertexBufferObject = glGenBuffer();
            glBindBuffer(GL_ARRAY_BUFFER, VertexBufferObject);

            //Fill the vertex buffer object with data about our vertices.
            fixed (float* v = &vertices[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, v, GL_STATIC_DRAW);
            }

            //Generate the UV buffer
            UvBuffer = glGenBuffer();
            glBindBuffer(GL_ARRAY_BUFFER, UvBuffer);

            //Generate the UV Buffer data
            fixed (float* u = &uvs[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * uvs.Length, u, GL_STATIC_DRAW);
            }

            Log.WriteLine($"Generated buffer for model, VAO: {VertexArrayObject}, VBO: {VertexBufferObject}, Length: {VerticesLength}", LogType.DEBUG);
        }

    }
}
