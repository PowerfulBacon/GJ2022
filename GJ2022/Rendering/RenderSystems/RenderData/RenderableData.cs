using GJ2022.Rendering.Models;
using GJ2022.Rendering.Shaders;
using GJ2022.Rendering.Textures;

namespace GJ2022.Rendering.RenderSystems.RenderData
{
    public class RenderableData
    {

        public ShaderSet shader;

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

        public RenderableData(ShaderSet shader, Model modelData, Renderable attachedRenderable, RendererTextureData textureData)
        {
            this.shader = shader;
            this.modelData = modelData;
            this.attachedRenderable = attachedRenderable;
            this.textureData = textureData;
        }
    }
}
