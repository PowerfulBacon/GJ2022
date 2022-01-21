using GJ2022.Rendering.RenderSystems.Interfaces;

namespace GJ2022.Rendering.RenderSystems
{
    /// <summary>
    /// Render batches have:
    ///  - Shared model data
    ///  - Shared texture data
    ///  - Unique positions
    ///  - Unique texture data offsets
    /// TODO: Should render systems handle the storing and fetching of data from its entities?
    /// </summary>
    public class RenderBatch<RenderInterface, TargetRenderSystem>
        //Render interface: The interface being stored used by the render system
        where RenderInterface : IInternalRenderable
        //Target render system: The render system type we are targetting
        where TargetRenderSystem : RenderSystem<RenderInterface, TargetRenderSystem>
    {

        public const int MAX_BATCH_SIZE = 25000;

        public RenderInterface[] instanceRenderables = new RenderInterface[MAX_BATCH_SIZE];

        public float[][] bufferArrays;

        private int[] bufferWidths;

        public RenderBatch(int bufferCount, int[] bufferWidths)
        {
            this.bufferWidths = bufferWidths;
            //Create the buffer arrays
            bufferArrays = new float[bufferCount][];
            //Fill the buffer arrays with proper widths
            for (int i = 0; i < bufferCount; i++)
            {
                bufferArrays[i] = new float[bufferWidths[i] * MAX_BATCH_SIZE];
            }
        }

        /// <summary>
        /// Updates an instance stored in this batch at the specified index
        /// </summary>
        public void UpdateInstance(int index)
        {
            for (int i = 0; i < bufferArrays.Length; i++)
            {
                RenderInterface target = instanceRenderables[i];
                TargetRenderSystem targetSystem = target.TargetRenderSystem;
                float[] bufferData = target.
                float[] bufferData = instanceRenderables[index].GetBufferData(i);
                UpdateBuffer(i, index, bufferData);
            }
        }

        /// <summary>
        /// Update a buffer at the specified index with the specified data.
        /// </summary>
        public void UpdateBuffer(int buffer, int index, float[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                bufferArrays[buffer][index * data.Length + i] = data[i];
            }
        }

        /// <summary>
        /// Copy this batch to another batch
        /// </summary>
        public void CopyTo(RenderBatch<RenderInterface, TargetRenderSystem> target, int sourceIndex, int targetIndex)
        {
            //Go through each buffer we need to copy
            for (int i = 0; i < bufferArrays.Length; i++)
            {
                //Get the width of this buffer
                int bufferWidth = bufferWidths[i];
                //Copy elements across
                for (int j = 0; j < bufferWidth; j++)
                {
                    target.bufferArrays[i][targetIndex * bufferWidth + j] = bufferArrays[i][sourceIndex * bufferWidth + j];
                }
            }
        }

    }
}
