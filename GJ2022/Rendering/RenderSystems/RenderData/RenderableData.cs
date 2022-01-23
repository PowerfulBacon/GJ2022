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
        /// Texture data
        /// </summary>
        public RendererTextureData textureData;

        public RenderableData(ShaderSet shader, Model modelData, RendererTextureData textureData)
        {
            this.shader = shader;
            this.modelData = modelData;
            this.textureData = textureData;
        }
    }
}
