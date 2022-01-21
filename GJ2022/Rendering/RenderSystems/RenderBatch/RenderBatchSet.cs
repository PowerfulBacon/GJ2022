using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;
using System.Linq;

namespace GJ2022.Rendering.RenderSystems
{
    public class RenderBatchSet
    {

        public List<RenderBatch> renderBatches = new List<RenderBatch>();

        public int renderElements = 0;

        //The texture data of the batch set
        public RendererTextureData textureData;

        public RenderBatchSet(RendererTextureData textureData)
        {
            this.textureData = textureData;
        }

        /// <summary>
        /// Adding to batches is simple, since we just stick it on the end.
        /// </summary>
        public void AddToBatch(IInstanceRenderable element, RendererTextureData textureData)
        {
            RenderBatch batchToAddTo;
            int positionInBatch = renderElements % RenderBatch.MAX_BATCH_SIZE;
            //We need to add a new render batch
            if (positionInBatch == 0)
            {
                batchToAddTo = new RenderBatch();
                renderBatches.Add(batchToAddTo);
                Log.WriteLine($"Created new rendering batch, there are now {renderBatches.Count} batches in this set.", LogType.DEBUG);
            }
            else
            {
                batchToAddTo = renderBatches.Last();
            }
            //Add to the batch
            batchToAddTo.instanceRenderables[positionInBatch] = element;
            element.SetRenderableBatchIndex(this, renderElements);
            //Cache all these values that are generally constant
            UpdateRenderablePosition(element);
            UpdateRenderableScales(element);
            UpdateRenderableGeneralData(element);
            batchToAddTo.batchSpriteData[positionInBatch * 4] = textureData.IndexX;
            batchToAddTo.batchSpriteData[positionInBatch * 4 + 1] = textureData.IndexY;
            batchToAddTo.batchSpriteData[positionInBatch * 4 + 2] = textureData.Width;
            batchToAddTo.batchSpriteData[positionInBatch * 4 + 3] = textureData.Height;
            renderElements++;
        }

        /// <summary>
        /// Updates the cache with the new position of the element.
        /// This must be called when a renderable element moves in the game world
        /// so the renderer world can update it as being moved.
        /// Renderer actual positions and theoretical positions are not directly linked
        /// so communication is needed between theoretical and physical systems.
        /// </summary>
        public void UpdateRenderablePosition(IInstanceRenderable element)
        {
            int locatedIndex = element.GetRenderableBatchIndex(this) % RenderBatch.MAX_BATCH_SIZE;
            RenderBatch batch = GetContainingBatch(element.GetRenderableBatchIndex(this));
            batch.batchPositionArray[locatedIndex * 3] = element.GetInstancePosition()[0];
            batch.batchPositionArray[locatedIndex * 3 + 1] = element.GetInstancePosition()[1];
            batch.batchPositionArray[locatedIndex * 3 + 2] = element.GetInstancePosition()[2];
        }

        /// <summary>
        /// Updates the cache with the new scale of the element.
        /// This must be called every single time a renderable element has its scale changed.
        /// </summary>
        public void UpdateRenderableScales(IInstanceRenderable element)
        {
            int locatedIndex = element.GetRenderableBatchIndex(this) % RenderBatch.MAX_BATCH_SIZE;
            RenderBatch batch = GetContainingBatch(element.GetRenderableBatchIndex(this));
            batch.batchSizeArray[locatedIndex * 2] = element.GetInstanceScale()[0];
            batch.batchSizeArray[locatedIndex * 2 + 1] = element.GetInstanceScale()[1];
        }

        public void UpdateRenderableGeneralData(IInstanceRenderable element)
        {
            int locatedIndex = element.GetRenderableBatchIndex(this) % RenderBatch.MAX_BATCH_SIZE;
            RenderBatch batch = GetContainingBatch(element.GetRenderableBatchIndex(this));
            Vector instanceData = element.GetInstanceGeneralData();
            batch.batchDataArray[locatedIndex * 4] = instanceData[0];
            batch.batchDataArray[locatedIndex * 4 + 1] = instanceData[1];
            batch.batchDataArray[locatedIndex * 4 + 2] = instanceData[2];
            batch.batchDataArray[locatedIndex * 4 + 3] = instanceData[3];
        }

        /// <summary>
        /// Removing from batches is a lot harder since we need to rearange things.
        /// As an optimisation, instead of moving everything, we just move the last element.
        /// O(n) [We could do O(1) but it would need more memory]
        /// </summary>
        /// <param name="element"></param>
        public void RemoveFromBatch(IInstanceRenderable element)
        {
            int locatedIndex = element.GetRenderableBatchIndex(this) % RenderBatch.MAX_BATCH_SIZE;
            RenderBatch batch = GetContainingBatch(element.GetRenderableBatchIndex(this));
            //Remove element from that batch
            int positionOfLast = (renderElements - 1) % RenderBatch.MAX_BATCH_SIZE;
            //Get the last renderable data element
            RenderBatch lastBatch = renderBatches.Last();
            //Move the element from the last index into this old position
            //Tell the element that it was moved
            batch.instanceRenderables[locatedIndex] = lastBatch.instanceRenderables[positionOfLast];
            batch.instanceRenderables[locatedIndex].SetRenderableBatchIndex(this, element.GetRenderableBatchIndex(this));
            //Copy the data from the last batch element into the removed batch element
            batch.batchPositionArray[locatedIndex * 3] = lastBatch.batchPositionArray[positionOfLast * 3];
            batch.batchPositionArray[locatedIndex * 3 + 1] = lastBatch.batchPositionArray[positionOfLast * 3 + 1];
            batch.batchPositionArray[locatedIndex * 3 + 2] = lastBatch.batchPositionArray[positionOfLast * 3 + 2];
            //Copy across texture data
            batch.batchSpriteData[locatedIndex * 4] = lastBatch.batchSpriteData[positionOfLast * 4];
            batch.batchSpriteData[locatedIndex * 4 + 1] = lastBatch.batchSpriteData[positionOfLast * 4 + 1];
            batch.batchSpriteData[locatedIndex * 4 + 2] = lastBatch.batchSpriteData[positionOfLast * 4 + 2];
            batch.batchSpriteData[locatedIndex * 4 + 3] = lastBatch.batchSpriteData[positionOfLast * 4 + 3];
            //Copy across the scale data
            batch.batchSizeArray[locatedIndex * 2] = lastBatch.batchSizeArray[positionOfLast * 2];
            batch.batchSizeArray[locatedIndex * 2 + 1] = lastBatch.batchSizeArray[positionOfLast * 2 + 1];
            //Copy across texture data
            batch.batchDataArray[locatedIndex * 4] = lastBatch.batchDataArray[positionOfLast * 4];
            batch.batchDataArray[locatedIndex * 4 + 1] = lastBatch.batchDataArray[positionOfLast * 4 + 1];
            batch.batchDataArray[locatedIndex * 4 + 2] = lastBatch.batchDataArray[positionOfLast * 4 + 2];
            batch.batchDataArray[locatedIndex * 4 + 3] = lastBatch.batchDataArray[positionOfLast * 4 + 3];
            //Decrease the render elements amount
            renderElements--;
            //If the position of the last element was the first in the array, remove the batch
            if (positionOfLast == 0)
            {
                renderBatches.Remove(renderBatches.Last());
            }
            Log.WriteLine($"Removed element from batch, new batch size: {renderElements}, new batch count: {renderBatches.Count}", LogType.DEBUG);
        }

        public RenderBatch GetContainingBatch(int index)
        {
            int batchIndex = index / RenderBatch.MAX_BATCH_SIZE;
            return renderBatches[batchIndex];
        }

    }
}
