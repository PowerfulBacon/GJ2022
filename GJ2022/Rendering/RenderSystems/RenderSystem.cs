using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.RenderSystems.RenderData;
using GJ2022.Rendering.Shaders;
using System.Collections.Generic;
using static OpenGL.Gl;

namespace GJ2022.Rendering.RenderSystems
{
    /// <summary>
    /// Render systems, handles rendering related stuff
    /// </summary>
    /// <typeparam name="RenderTargetInterface">The interface used by things rendered by this system</typeparam>
    public abstract class RenderSystem<RenderTargetInterface, TargetRenderSystem>
        where RenderTargetInterface : IInternalRenderable
        where TargetRenderSystem : RenderSystem<RenderTargetInterface, TargetRenderSystem>
    {

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
        protected string[] UniformVariableNames => new string[] {
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

        /// <summary>
        /// Returns the buffer data for a specified renderable.
        /// </summary>
        public abstract float[] GetBufferData(RenderTargetInterface renderableInterface);

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
        public virtual unsafe void BeginRender(Camera mainCamera)
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
        public virtual void RenderModels(Camera mainCamera)
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

                //Bind the buffers
                for (uint i = 0; i < BufferCount; i++)
                {
                    BindAttribArray(i, bufferLocations[i], BufferWidths[i]);
                    glVertexAttribDivisor(i, BufferDataPointsPerInstance[i]);
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
                    int count = i == renderBatchSet.renderBatches.Count - 1 ? renderBatchSet.renderElements % RenderBatch<RenderTargetInterface, TargetRenderSystem>.MAX_BATCH_SIZE : RenderBatch<RenderTargetInterface, TargetRenderSystem>.MAX_BATCH_SIZE;

                    //Now that we have the instance positions in an array, we
                    //can send it to openGL and render.
                    fixed (float* instancePositionArrayPointer = &batch.batchPositionArray[0])
                    {
                        glBindBuffer(GL_ARRAY_BUFFER, instancePositionBuffer);
                        glBufferData(GL_ARRAY_BUFFER, sizeof(float) * 3 * RenderBatch.MAX_BATCH_SIZE, NULL, GL_STREAM_DRAW);
                        glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(float) * 3 * count, instancePositionArrayPointer);
                    }

                    //Do the same for texture data
                    fixed (float* instanceTexDataArrayPointer = &batch.batchSpriteData[0])
                    {
                        glBindBuffer(GL_ARRAY_BUFFER, instanceTexDataBuffer);
                        glBufferData(GL_ARRAY_BUFFER, sizeof(float) * 4 * RenderBatch.MAX_BATCH_SIZE, NULL, GL_STREAM_DRAW);
                        glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(float) * 4 * count, instanceTexDataArrayPointer);
                    }

                    //Finally, do the same for scaling data
                    fixed (float* instanceScaleArrayPointer = &batch.batchSizeArray[0])
                    {
                        glBindBuffer(GL_ARRAY_BUFFER, instanceScaleDataBuffer);
                        glBufferData(GL_ARRAY_BUFFER, sizeof(float) * 2 * RenderBatch.MAX_BATCH_SIZE, NULL, GL_STREAM_DRAW);
                        glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(float) * 2 * count, instanceScaleArrayPointer);
                    }

                    //Perform batch rendering
                    //6 vertices so count of 6.
                    glDrawArraysInstanced(GL_TRIANGLES, 0, cacheKey.Model.VerticesLength, count);

                }

                //Disable the vertex arrays
                glDisableVertexAttribArray(0);
                glDisableVertexAttribArray(1);
                glDisableVertexAttribArray(2);
                glDisableVertexAttribArray(3);
                glDisableVertexAttribArray(4);
            }

        }

        protected virtual unsafe void BindUniformData(RenderBatchSet<RenderTargetInterface, TargetRenderSystem> renderBatchSet)
        {
            glUniform1i(uniformVariableLocations["textureSampler"], 0);
            glUniform1f(uniformVariableLocations["spriteWidth"], renderBatchSet.textureData.FileWidth);
            glUniform1f(uniformVariableLocations["spriteHeight"], renderBatchSet.textureData.FileHeight);
        }

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
