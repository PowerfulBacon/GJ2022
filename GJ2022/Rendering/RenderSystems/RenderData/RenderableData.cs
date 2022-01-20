using GJ2022.Rendering.Models;
using GJ2022.Rendering.Textures;

namespace GJ2022.Rendering.RenderSystems.RenderData
{
    public class RenderableData
    {

        /// <summary>
        /// Specific model data
        /// </summary>
        public Model modelData;

        /// <summary>
        /// Positional data
        /// </summary>
        public Renderable attachedRenderable;

        /// <summary>
        /// Texture data
        /// </summary>
        public RendererTextureData textureData;

        public RenderableData(Model modelData, Renderable attachedRenderable, RendererTextureData textureData)
        {
            this.modelData = modelData;
            this.attachedRenderable = attachedRenderable;
            this.textureData = textureData;
        }
    }
}
