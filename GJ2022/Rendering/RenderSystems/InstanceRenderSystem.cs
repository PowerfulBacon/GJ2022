using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.RenderSystems.RenderData;
using static OpenGL.Gl;

namespace GJ2022.Rendering.RenderSystems
{
    /// <summary>
    /// Rendering system optimised for batch rendering
    /// tons of the same object.
    /// </summary>
    public class InstanceRenderSystem : RenderSystem
    {

        public static InstanceRenderSystem Singleton;

        //Our shaders are the instance shader set
        protected override string SystemShaderName => "instanceShader";

        //The location of the instance position buffer.
        //Instance positions are sent via the buffer; the high speed
        //data transfer to the GPU.
        private uint instancePositionBuffer;

        //Location of the instance texture data buffer.
        private uint instanceTexDataBuffer;

        //Location of uniform variables
        private int viewMatrixUniformLocation;
        private int projectionMatrixUniformLocation;
        private int textureSamplerUniformLocation;
        private int spriteWidthUniformLocation;
        private int spriteHeightUniformLocation;

        public InstanceRenderSystem()
        {
            Singleton = this;
        }

        /// <summary>
        /// Start rendering the models of the renderable provided.
        /// TODO: Make this abstract and make it so not all rendering systems are required to use instance renderables
        /// </summary>
        public void StartRendering(Renderable renderable)
        {
            renderable.IsRendering = true;
            //Begin rendering
            foreach (RenderableData renderableData in renderable.ModelData.GetModelRenderableData(renderable))
            {
                StartRendering(renderableData);
            }
        }

        /// <summary>
        /// Start rendering a specific renderable data
        /// </summary>
        private void StartRendering(RenderableData renderableData)
        {
            //Todo: Convert this to a struct
            RenderBatchGroup renderCacheKey = new RenderBatchGroup(renderableData.shader, renderableData.modelData, renderableData.textureData.TextureUint);
            if (renderCache.ContainsKey(renderCacheKey))
            {
                renderCache[renderCacheKey].AddToBatch(renderableData.attachedRenderable as IInstanceRenderable, renderableData.textureData);
            }
            else
            {
                RenderBatchSet batchSet = new RenderBatchSet(renderableData.textureData);
                batchSet.AddToBatch(renderableData.attachedRenderable as IInstanceRenderable, renderableData.textureData);
                renderCache.Add(renderCacheKey, batchSet);
            }
        }

        /// <summary>
        /// Stop rendering the renderable provided.
        /// </summary>
        public void StopRendering(Renderable renderable)
        {
            renderable.IsRendering = false;
            //Assumes that renderable is actually in the renderCache
            //:oblivious:
            foreach (RenderableData renderableData in renderable.ModelData.GetModelRenderableData(renderable))
            {
                StopRendering(renderableData);
            }
        }

        public void StopRendering(RenderableData renderableData)
        {
            RenderBatchGroup renderCacheKey = new RenderBatchGroup(renderableData.shader, renderableData.modelData, renderableData.textureData.TextureUint);
            if (!renderCache.ContainsKey(renderCacheKey))
            {
                Log.WriteLine($"Failed to locate key in render dict", LogType.WARNING);
                return;
            }
            renderCache[renderCacheKey].RemoveFromBatch(renderableData.attachedRenderable as IInstanceRenderable);
            if (renderCache[renderCacheKey].renderElements == 0)
            {
                renderCache.Remove(renderCacheKey);
            }
        }

        public unsafe override void Initialize()
        {

            //Generate an empty buffer for the instance position buffer
            instancePositionBuffer = glGenBuffer();
            glBindBuffer(GL_ARRAY_BUFFER, instancePositionBuffer);
            //Populate with empty data (We are just reserving the space)
            glBufferData(GL_ARRAY_BUFFER, sizeof(float) * 3 * RenderBatch.MAX_BATCH_SIZE, NULL, GL_STREAM_DRAW);

            //Do the same for instance texture data
            instanceTexDataBuffer = glGenBuffer();
            glBindBuffer(GL_ARRAY_BUFFER, instanceTexDataBuffer);
            //Populate with empty data (We are just reserving the space)
            glBufferData(GL_ARRAY_BUFFER, sizeof(float) * 4 * RenderBatch.MAX_BATCH_SIZE, NULL, GL_STREAM_DRAW);
        }

        public unsafe override void BeginRender(Camera mainCamera)
        {

            //Attach the shader set
            glUseProgram(programUint);

            //Load the camera's view matrix
            //Put the matrix into that uniform variable
            glUniformMatrix4fv(viewMatrixUniformLocation, 1, false, mainCamera.ViewMatrix.GetPointer());

            //Load the camera's projection matrix
            //Put the matrix into that uniform variable
            glUniformMatrix4fv(projectionMatrixUniformLocation, 1, false, mainCamera.ProjectionMatrix.GetPointer());
        }

        //Reuse the same arrays over and over, we don't need to change their size.
        float[] instancePositionArray = new float[3 * RenderBatch.MAX_BATCH_SIZE];
        float[] spriteSheetOffsets = new float[4 * RenderBatch.MAX_BATCH_SIZE];  //vec4(x, y, width, height)

        public unsafe override void RenderModels()
        {

            uint loadedShader = uint.MaxValue;

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

                if (loadedShader != cacheKey.Shaders.GetVertexShader())
                {
                    //Attach the shader set so we can grab uniform locations
                    //Link program and use program are required here, not sure what they do exactly.
                    cacheKey.Shaders.AttachShaders(programUint);
                    glLinkProgram(programUint);

                    //Get the uniform locations
                    viewMatrixUniformLocation = glGetUniformLocation(programUint, "viewMatrix");
                    projectionMatrixUniformLocation = glGetUniformLocation(programUint, "projectionMatrix");
                    textureSamplerUniformLocation = glGetUniformLocation(programUint, "textureSampler");
                    spriteWidthUniformLocation = glGetUniformLocation(programUint, "spriteWidth");
                    spriteHeightUniformLocation = glGetUniformLocation(programUint, "spriteHeight");
                    //Detatch the shaders
                    cacheKey.Shaders.DetatchShaders(programUint);

                    loadedShader = cacheKey.Shaders.GetVertexShader();
                }

                //================
                //SHARED BETWEEN BATCHES
                //================

                //Get a list of all things to render with this model
                RenderBatchSet renderBatchSet = renderCache[cacheKey];
                //Bind the vertex buffer object and the vertex array object
                BindAttribArray(0, cacheKey.Model.VertexBufferObject, 3);
                BindAttribArray(1, cacheKey.Model.UvBuffer, 2);

                //Enable the 2nd vertex attrib array
                BindAttribArray(2, instancePositionBuffer, 3);
                //Enable the 3rd vertex attrib array
                BindAttribArray(3, instanceTexDataBuffer, 4);

                //Set the vertex attrib divisors
                //Always reuse the provided vertices, so don't increment
                glVertexAttribDivisor(0, 0);
                //Reuse the UVs for each instance
                glVertexAttribDivisor(1, 0);
                //1 position per instancei
                glVertexAttribDivisor(2, 1);
                //1 position per instance
                glVertexAttribDivisor(3, 1);

                //Load in the textures
                glBindTexture(GL_TEXTURE0, cacheKey.TextureUint);
                glUniform1i(textureSamplerUniformLocation, 0);
                glUniform1f(spriteWidthUniformLocation, renderBatchSet.textureData.FileWidth);
                glUniform1f(spriteHeightUniformLocation, renderBatchSet.textureData.FileHeight);

                //================
                // Process each batch
                //================

                for (int i = renderBatchSet.renderBatches.Count - 1; i >= 0; i--)
                {

                    //Get the batch we are rendering
                    RenderBatch batch = renderBatchSet.renderBatches[i];

                    //Get the count of elements in this batch
                    //If its the end batch, its the amount of render elements mod the batch size,
                    //if its not the end batch then its the batch size (Non-last batches are full)
                    int count = i == renderBatchSet.renderBatches.Count - 1 ? renderBatchSet.renderElements % RenderBatch.MAX_BATCH_SIZE : RenderBatch.MAX_BATCH_SIZE;

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

                    //Perform batch rendering
                    //6 vertices so count of 6.
                    glDrawArraysInstanced(GL_TRIANGLES, 0, cacheKey.Model.VerticesLength, count);
                }

                //Disable the vertex arrays
                glDisableVertexAttribArray(0);
                glDisableVertexAttribArray(1);
                glDisableVertexAttribArray(2);
                glDisableVertexAttribArray(3);
            }
        }

        public override void EndRender()
        { }

    }
}
