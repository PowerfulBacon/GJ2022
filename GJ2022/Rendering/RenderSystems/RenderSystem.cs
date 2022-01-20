using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.RenderSystems.RenderData;
using GJ2022.Rendering.Shaders;
using System.Collections.Generic;
using static OpenGL.Gl;

namespace GJ2022.Rendering.RenderSystems
{
    public abstract class RenderSystem
    {

        //The cache of things we are rendering
        //Key: ModelData (objects with the same modeldata should reference the same class)
        //Value: List of Renderables being rendered with that model data
        protected Dictionary<RenderBatchGroup, RenderBatchSet> renderCache = new Dictionary<RenderBatchGroup, RenderBatchSet>();

        //Name of the system shader
        protected abstract string SystemShaderName { get; }

        //Shaders used by the render system
        protected ShaderSet SystemShaders { get; }

        protected uint programUint;

        public RenderSystem()
        {
            SystemShaders = new ShaderSet(SystemShaderName);

            programUint = glCreateProgram();
            Log.WriteLine($"Created new program: ID {programUint}", LogType.DEBUG);

            Initialize();
        }

        /// <summary>
        /// Initialize the rendering system
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Prepare the system for rendering
        /// </summary>
        public abstract void BeginRender(Camera mainCamera);

        /// <summary>
        /// Render the models provided.
        /// Requires the ModelData instance and a list of renderable objects associated with that
        /// </summary>
        public abstract void RenderModels();

        /// <summary>
        /// Cleanup anything we did in beginRender
        /// </summary>
        public abstract void EndRender();

        /// <summary>
        /// Binds the atrib array at index provided, with the buffer data provided.
        /// </summary>
        protected unsafe void BindAttribArray(uint index, uint buffer, int size)
        {
            glEnableVertexAttribArray(index);
            glBindBuffer(GL_ARRAY_BUFFER, buffer);
            glVertexAttribPointer(
                index,              //Attribute - Where the layout location is in the vertex shader
                size,               //Size of the triangles (3 sides)
                GL_FLOAT,           //Type (Floats)
                false,              //Normalized (nope)
                0,                  //Stride (0)
                NULL                //Array buffer offset (null)
            );
        }

    }
}
