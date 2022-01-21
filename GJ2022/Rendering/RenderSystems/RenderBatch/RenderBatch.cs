using GJ2022.Rendering.RenderSystems.Interfaces;

namespace GJ2022.Rendering.RenderSystems
{
    /// <summary>
    /// Render batches have:
    ///  - Shared model data
    ///  - Shared texture data
    ///  - Unique positions
    ///  - Unique texture data offsets
    /// </summary>
    public class RenderBatch
    {

        public const int MAX_BATCH_SIZE = 25000;

        public IInstanceRenderable[] instanceRenderables = new IInstanceRenderable[MAX_BATCH_SIZE];

        public float[] batchPositionArray = new float[3 * MAX_BATCH_SIZE];

        public float[] batchSpriteData = new float[4 * MAX_BATCH_SIZE];

        public float[] batchSizeArray = new float[2 * MAX_BATCH_SIZE];

    }
}
