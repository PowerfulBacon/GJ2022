using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.Rendering.RenderSystems.Renderables
{
    public abstract class Renderable : IInternalRenderable
    {

        //Delegates
        public delegate void MoveDelegate(Vector<float> position);
        public delegate void LayerChangeDelegate(float layer);
        public delegate void TextureChangeDelegate(string texture);

        //Events
        public abstract MoveDelegate moveHandler { get; }
        public abstract LayerChangeDelegate layerChangeHandler { get; }
        public abstract TextureChangeDelegate textureChangeHandler { get; }

        public abstract RendererTextureData GetRendererTextureData();

        protected Dictionary<object, int> renderableBatchIndex = new Dictionary<object, int>();

        public abstract void StopRendering();
        public abstract void StartRendering();
        public abstract void ContinueRendering();
        public abstract void PauseRendering();

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

    }
}
