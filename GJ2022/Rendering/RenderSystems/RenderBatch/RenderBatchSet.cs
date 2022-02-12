using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using System.Collections.Generic;
using System.Linq;

namespace GJ2022.Rendering.RenderSystems
{
    public class RenderBatchSet<RenderInterface, TargetRenderSystem>
        //Render interface: The interface being stored used by the render system
        where RenderInterface : IInternalRenderable
        //Target render system: The render system type we are targetting
        where TargetRenderSystem : RenderSystem<RenderInterface, TargetRenderSystem>
    {

        public List<RenderBatch<RenderInterface, TargetRenderSystem>> renderBatches = new List<RenderBatch<RenderInterface, TargetRenderSystem>>();

        public int renderElements = 0;

        //The texture data of the batch set
        public RendererTextureData textureData;

        //The buffer count of each render batch
        private int bufferCount;

        //The buffer widths for each render batch
        private int[] bufferWidths;

        public RenderBatchSet(RendererTextureData textureData, int bufferCount, int[] bufferWidths)
        {
            this.textureData = textureData;
            this.bufferCount = bufferCount;
            this.bufferWidths = bufferWidths;
        }

        /// <summary>
        /// Updates batch data, allowing data changes
        /// </summary>
        public void UpdateBatchData(RenderInterface target, int bufferIndex)
        {
            int locatedIndex = target.GetRenderableBatchIndex(this) % RenderBatch<RenderInterface, TargetRenderSystem>.MAX_BATCH_SIZE;
            RenderBatch<RenderInterface, TargetRenderSystem> batch = GetContainingBatch(target.GetRenderableBatchIndex(this));
            batch.UpdateBuffer(bufferIndex, locatedIndex, (target as IInstanceRenderable<RenderInterface, TargetRenderSystem>).RenderSystem.GetBufferData(target, bufferIndex));
        }

        /// <summary>
        /// Adding to batches is simple, since we just stick it on the end.
        /// </summary>
        public void AddToBatch(RenderInterface element, RendererTextureData textureData)
        {
            RenderBatch<RenderInterface, TargetRenderSystem> batchToAddTo;
            int positionInBatch = renderElements % RenderBatch<RenderInterface, TargetRenderSystem>.MAX_BATCH_SIZE;
            //We need to add a new render batch
            if (positionInBatch == 0)
            {
                batchToAddTo = new RenderBatch<RenderInterface, TargetRenderSystem>(bufferCount, bufferWidths);
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
            //Cache the buffer values of the provided instance renderable
            batchToAddTo.UpdateInstance(positionInBatch);
            //Another render element has been added
            renderElements++;
        }

        /// <summary>
        /// Removing from batches is a lot harder since we need to rearange things.
        /// As an optimisation, instead of moving everything, we just move the last element.
        /// O(n) [We could do O(1) but it would need more memory]
        /// </summary>
        /// <param name="element"></param>
        public void RemoveFromBatch(RenderInterface element)
        {
            int locatedIndex = element.GetRenderableBatchIndex(this) % RenderBatch<RenderInterface, TargetRenderSystem>.MAX_BATCH_SIZE;
            RenderBatch<RenderInterface, TargetRenderSystem> batch = GetContainingBatch(element.GetRenderableBatchIndex(this));
            //Remove element from that batch
            int positionOfLast = (renderElements - 1) % RenderBatch<RenderInterface, TargetRenderSystem>.MAX_BATCH_SIZE;
            //Get the last renderable data element
            RenderBatch<RenderInterface, TargetRenderSystem> lastBatch = renderBatches.Last();
            //Move the element from the last index into this old position
            //Tell the element that it was moved
            batch.instanceRenderables[locatedIndex] = lastBatch.instanceRenderables[positionOfLast];
            batch.instanceRenderables[locatedIndex].SetRenderableBatchIndex(this, element.GetRenderableBatchIndex(this));
            //Reset the index
            element.ClearRenderableBatchIndex(this);
            //Copy data from the last batch element into the current one
            lastBatch.CopyTo(batch, positionOfLast, locatedIndex);
            //Decrease the render elements amount
            renderElements--;
            //If the position of the last element was the first in the array, remove the batch
            if (positionOfLast == 0)
            {
                renderBatches.Remove(renderBatches.Last());
                Log.WriteLine($"Removed element from batch (+removed a batch), new batch size: {renderElements}, new batch count: {renderBatches.Count}", LogType.DEBUG);
            }
        }

        public RenderBatch<RenderInterface, TargetRenderSystem> GetContainingBatch(int index)
        {
            int batchIndex = index / RenderBatch<RenderInterface, TargetRenderSystem>.MAX_BATCH_SIZE;
            return renderBatches[batchIndex];
        }

    }
}
