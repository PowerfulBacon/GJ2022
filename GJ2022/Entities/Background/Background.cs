using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.Entities.Background
{
    class BackgroundEntity : Entity, IBackgroundRenderable
    {

        public override ModelData ModelData { get; set; } = QuadModelData.Singleton;

        public RenderSystem<IBackgroundRenderable, BackgroundRenderSystem> RenderSystem => BackgroundRenderSystem.Singleton;

        public BackgroundEntity(Vector position) : base(position)
        { }

        /// <summary>
        /// Instance renderable stuff
        /// </summary>

        private Dictionary<object, int> renderableBatchIndex = new Dictionary<object, int>();

        public void SetRenderableBatchIndex(object associatedSet, int index)
        {
            if (renderableBatchIndex.ContainsKey(associatedSet))
                renderableBatchIndex[associatedSet] = index;
            else
                renderableBatchIndex.Add(associatedSet, index);
        }

        /// <summary>
        /// Returns the renderable batch index in the provided set.
        /// Returns -1 if failed.
        /// </summary>
        public int GetRenderableBatchIndex(object associatedSet)
        {
            if (renderableBatchIndex.ContainsKey(associatedSet))
                return renderableBatchIndex[associatedSet];
            else
                return -1;
        }

        public RendererTextureData GetRendererTextureData()
        {
            return TextureCache.GetTexture(TextureCache.ERROR_ICON_STATE);
        }
    }
}
