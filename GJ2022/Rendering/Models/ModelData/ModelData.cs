using GJ2022.Rendering.RenderSystems.RenderData;

namespace GJ2022.Rendering.Models
{
    /// <summary>
    /// Since these are used as keys in the renderer's dictionary
    /// of things to render, a single modelData should be referenced
    /// by all the renderables that use it.
    /// </summary>
    public class ModelData
    {

        private Model model;

        public ModelData(Model model)
        {
            this.model = model;
        }

        /// <summary>
        /// Returns a model based on what sides the block is blocked on.
        /// </summary>
        /// <param name="renderBlockFlag"></param>
        public virtual RenderableData[] GetModelRenderableData(Renderable renderable, CubeFaceFlags faceFlag)
        {
            return new RenderableData[] { new RenderableData(model, renderable, renderable.GetRendererTexture(faceFlag)) };
        }

        /// <summary>
        /// Returns an array containing all the models rendered by
        /// this model data object.
        /// Cubes return 6 models, 1 for each face.
        /// </summary>
        public virtual RenderableData[] GetAllModelRenderableData(Renderable renderable)
        {
            return new RenderableData[] { new RenderableData(model, renderable, renderable.GetRendererTexture(CubeFaceFlags.FACE_FRONT)) };
        }

    }
}
