using GJ2022.Rendering.RenderSystems.RenderData;
using GJ2022.Rendering.Shaders;

namespace GJ2022.Rendering.Models
{
    /// <summary>
    /// Since these are used as keys in the renderer's dictionary
    /// of things to render, a single modelData should be referenced
    /// by all the renderables that use it.
    /// </summary>
    public class ModelData
    {

        protected ShaderSet shader;

        private Model model;

        public ModelData(ShaderSet shader, Model model)
        {
            this.shader = shader;
            this.model = model;
        }

        /// <summary>
        /// Returns a model based on what sides the block is blocked on.
        /// </summary>
        /// <param name="renderBlockFlag"></param>
        public virtual RenderableData[] GetModelRenderableData(Renderable renderable)
        {
            return new RenderableData[] { new RenderableData(shader, model, renderable, renderable.GetRendererTexture()) };
        }

    }
}
