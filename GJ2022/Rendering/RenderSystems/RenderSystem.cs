using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Shaders;
using System.Collections.Generic;
using static OpenGL.Gl;

namespace GJ2022.Rendering.RenderSystems
{

    public abstract class RenderSystem
    {
        public abstract unsafe void BeginRender(Camera mainCamera);
        public abstract unsafe void RenderModels(Camera mainCamera);
        public abstract void EndRender();
    }

    /// <summary>
    /// Render systems, handles rendering related stuff
    /// </summary>
    /// <typeparam name="RenderTargetInterface">The interface used by things rendered by this system</typeparam>
    public abstract class RenderSystem<RenderTargetInterface, TargetRenderSystem> : RenderSystem
        where RenderTargetInterface : IInternalRenderable
        where TargetRenderSystem : RenderSystem<RenderTargetInterface, TargetRenderSystem>
    {

        private const int USER_BUFFER_OFFSET = 2;

        //The cache of things we are rendering
        //Key: ModelData (objects with the same modeldata should reference the same class)
        //Value: List of Renderables being rendered with that model data
        protected virtual Dictionary<RenderBatchGroup, RenderBatchSet<RenderTargetInterface, TargetRenderSystem>> renderCache { get; } = new Dictionary<RenderBatchGroup, RenderBatchSet<RenderTargetInterface, TargetRenderSystem>>();

        //Name of the system shader
        protected abstract string SystemShaderName { get; }

        //Shaders used by the render system
        protected ShaderSet SystemShaders { get; }

        //Program uint used by the rendering system
        protected uint programUint;

        //The count of buffers this render system uses
        protected abstract int BufferCount { get; }

        //The array containing how wide each buffer is.
        //Buffers must be between 1 and 4 wide.
        protected abstract int[] BufferWidths { get; }

        //The attrib divisor for each buffer.
        //0 indicates that the buffer data is shared between all instances (models, textures)
        //1 indicates that each instance gets its own set of data (positions, colours etc.)
        protected abstract uint[] BufferDataPointsPerInstance { get; }

        //The buffer locations
        private uint[] bufferLocations;

        //The names of the uniform variables used by this render system
        protected virtual string[] UniformVariableNames => new string[] {
            "viewMatrix",
            "projectionMatrix",
            "textureSampler",
            "spriteWidth",
            "spriteHeight"
        };

        //The uniform variable locations
        protected Dictionary<string, int> uniformVariableLocations = new Dictionary<string, int>();

        public RenderSystem()
        {
            //Create the shader set
            SystemShaders = new ShaderSet(SystemShaderName);

            //Set the singleton
            SetSingleton();

            //Create the program used by this renderer
            programUint = glCreateProgram();
            Log.WriteLine($"Created new program: ID {programUint}", LogType.DEBUG);

            //Call initialization callback
            Initialize();
        }

        /// <summary>
        /// Start rendering the provided renderable object
        /// </summary>
        public virtual void StartRendering(RenderTargetInterface renderable)
        {
            RenderBatchGroup renderCacheKey = GetBatchGroup(renderable);
            if (renderCache.ContainsKey(renderCacheKey))
            {
                renderCache[renderCacheKey].AddToBatch(renderable, renderable.GetRendererTextureData());
            }
            else
            {
                RenderBatchSet<RenderTargetInterface, TargetRenderSystem> batchSet = new RenderBatchSet<RenderTargetInterface, TargetRenderSystem>(renderable.GetRendererTextureData(), BufferCount, BufferWidths);
                batchSet.AddToBatch(renderable, renderable.GetRendererTextureData());
                renderCache.Add(renderCacheKey, batchSet);
            }
        }

        /// <summary>
        /// Stop rendering the target instance
        /// </summary>
        public virtual void StopRendering(RenderTargetInterface renderable)
        {
            RenderBatchGroup renderCacheKey = GetBatchGroup(renderable);
            if (!renderCache.ContainsKey(renderCacheKey))
            {
                Log.WriteLine($"Failed to locate key in render dict", LogType.WARNING);
                return;
            }
            renderCache[renderCacheKey].RemoveFromBatch(renderable);
            //If the list stored at the key position is now empty, remove it from the render batch cache.
            if (renderCache[renderCacheKey].renderElements == 0)
            {
                renderCache.Remove(renderCacheKey);
            }
        }

        /// <summary>
        /// Generates the render batch group from a provided T class
        /// </summary>
        protected abstract RenderBatchGroup GetBatchGroup(RenderTargetInterface renderable);

        /// <summary>
        /// Set the singleton instance of the rendering system.
        /// </summary>
        protected abstract void SetSingleton();

        public abstract float[] GetBufferData(RenderTargetInterface targetItem, int bufferIndex);

        /// <summary>
        /// Provides the pointer to the buffer of data at the specified index.
        /// Fetches it from the buffer arrays.
        /// </summary>
        public virtual unsafe float* GetBufferPointer(RenderBatch<RenderTargetInterface, TargetRenderSystem> targetBatch, int index)
        {
            fixed (float* ptr = &targetBatch.bufferArrays[index][0])
                return ptr;
        }

        /// <summary>
        /// Initialize the rendering system.
        /// Bind all required buffers
        /// </summary>
        public virtual unsafe void Initialize()
        {

            bufferLocations = new uint[BufferCount];

            for (int i = 0; i < BufferCount; i++)
            {
                //Generate a buffer and store it in the appropriate location
                bufferLocations[i] = glGenBuffer();
                //Bind the buffer
                glBindBuffer(GL_ARRAY_BUFFER, bufferLocations[i]);
                //Reserve space in that buffer
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * BufferWidths[i] * RenderBatch<RenderTargetInterface, TargetRenderSystem>.MAX_BATCH_SIZE, NULL, GL_STREAM_DRAW);
            }

            //Uniform variable collection
            //Attach the shader set
            glUseProgram(programUint);
            //Attach the shader set so we can grab uniform locations
            //Link program and use program are required here, not sure what they do exactly.
            SystemShaders.AttachShaders(programUint);
            glLinkProgram(programUint);
            LoadUniformLocations();
            //Detatch the shaders
            SystemShaders.DetatchShaders(programUint);
        }

        /// <summary>
        /// Prepare the system for rendering
        /// </summary>
        public override unsafe void BeginRender(Camera mainCamera)
        {
            //Attach the shader set
            glUseProgram(programUint);

            //Attach the shader set so we can grab uniform locations
            //Link program and use program are required here, not sure what they do exactly.
            SystemShaders.AttachShaders(programUint);
            glLinkProgram(programUint);

            //Load the camera's view matrix
            //Put the matrix into that uniform variable
            glUniformMatrix4fv(uniformVariableLocations["viewMatrix"], 1, false, mainCamera.ViewMatrix.GetPointer());

            //Load the camera's projection matrix
            //Put the matrix into that uniform variable
            glUniformMatrix4fv(uniformVariableLocations["projectionMatrix"], 1, false, mainCamera.ProjectionMatrix.GetPointer());

            //Detatch the shaders
            SystemShaders.DetatchShaders(programUint);
        }

        private unsafe void LoadUniformLocations()
        {
            //Reset just in case
            uniformVariableLocations.Clear();
            //Load all uniform variable locations
            foreach (string uniformVariableName in UniformVariableNames)
            {
                uniformVariableLocations.Add(uniformVariableName, glGetUniformLocation(programUint, uniformVariableName));
            }
        }

        /// <summary>
        /// Render the models provided.
        /// Requires the ModelData instance and a list of renderable objects associated with that
        /// </summary>
        public override unsafe void RenderModels(Camera mainCamera)
        {
            //Generate an array for the positions of the things we're rendering
            //Bind the attrib arrays for each model
            //Render the batch
            //Disable the attrib arrays

            //Bind the instance buffer (We only need to bind it once and
            //reuse it)
            //glBindBuffer(GL_ARRAY_BUFFER, instancePositionBuffer);
            //Buffer orphaning
            //glBufferData(GL_ARRAY_BUFFER, sizeof(float) * 3 * MAX_BATCH_SIZE, NULL, GL_STREAM_DRAW);

            //Loop through each key of the dictionary
            //TODO: Potential concurrent modification exception
            foreach (RenderBatchGroup cacheKey in renderCache.Keys)
            {

                //================
                //SHARED BETWEEN BATCHES
                //================

                //Get a list of all things to render with this model
                RenderBatchSet<RenderTargetInterface, TargetRenderSystem> renderBatchSet = renderCache[cacheKey];

                //Bind the model and UV buffer
                BindAttribArray(0, cacheKey.Model.VertexBufferObject, 3);
                BindAttribArray(1, cacheKey.Model.UvBuffer, 2);

                glVertexAttribDivisor(0, 0);
                glVertexAttribDivisor(1, 0);

                //Bind the buffers
                for (uint i = 0; i < BufferCount; i++)
                {
                    BindAttribArray(i + USER_BUFFER_OFFSET, bufferLocations[i], BufferWidths[i]);
                    glVertexAttribDivisor(i + USER_BUFFER_OFFSET, BufferDataPointsPerInstance[i]);
                }

                //Load in the textures
                glBindTexture(GL_TEXTURE0, cacheKey.TextureUint);
                BindUniformData(renderBatchSet);

                //================
                // Process each batch
                //================

                for (int i = renderBatchSet.renderBatches.Count - 1; i >= 0; i--)
                {

                    //Get the batch we are rendering
                    RenderBatch<RenderTargetInterface, TargetRenderSystem> batch = renderBatchSet.renderBatches[i];

                    //Get the count of elements in this batch
                    //If its the end batch, its the amount of render elements mod the batch size,
                    //if its not the end batch then its the batch size (Non-last batches are full)
                    int count =
                        i == renderBatchSet.renderBatches.Count - 1
                        ? renderBatchSet.renderElements % RenderBatch<RenderTargetInterface, TargetRenderSystem>.MAX_BATCH_SIZE
                        : RenderBatch<RenderTargetInterface, TargetRenderSystem>.MAX_BATCH_SIZE;

                    //Correction for when N = max size, since if there are max size elements in the list, instead of rendering 0 at the end
                    //we want to render all of them.
                    if (count == 0 && renderBatchSet.renderBatches.Count != 0)
                    {
                        count = RenderBatch<RenderTargetInterface, TargetRenderSystem>.MAX_BATCH_SIZE;
                    }

                    //We only need to do this buffering on buffers that change
                    for (int bufferI = 0; bufferI < BufferCount; bufferI++)
                    {
                        //Otherwise provide the pointer to the array
                        float* bufferPointer = GetBufferPointer(batch, bufferI);
                        glBindBuffer(GL_ARRAY_BUFFER, bufferLocations[bufferI]);
                        glBufferData(GL_ARRAY_BUFFER, sizeof(float) * BufferWidths[bufferI] * RenderBatch<RenderTargetInterface, TargetRenderSystem>.MAX_BATCH_SIZE, NULL, GL_STREAM_DRAW);
                        glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(float) * BufferWidths[bufferI] * count, bufferPointer);
                    }

                    //Perform batch rendering
                    //6 vertices so count of 6.
                    glDrawArraysInstanced(GL_TRIANGLES, 0, cacheKey.Model.VerticesLength, count);

                }

                //Disable the vertex arrays
                for (uint i = 0; i < BufferCount + USER_BUFFER_OFFSET; i++)
                {
                    glDisableVertexAttribArray(i);
                }
            }

        }

        protected virtual unsafe void BindUniformData(RenderBatchSet<RenderTargetInterface, TargetRenderSystem> renderBatchSet)
        {
            glUniform1i(uniformVariableLocations["textureSampler"], 0);
            glUniform1f(uniformVariableLocations["spriteWidth"], renderBatchSet.textureData.FileWidth);
            glUniform1f(uniformVariableLocations["spriteHeight"], renderBatchSet.textureData.FileHeight);
        }

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
