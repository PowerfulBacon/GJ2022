using GJ2022.Atmospherics.Block;
using GJ2022.Entities.Items.Clothing.Back;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.Entities.Turfs.Standard
{
    public abstract class StandardRenderableTurf : Turf, IStandardRenderable
    {

        //Texture of the turf
        protected abstract string Texture { get; }

        public RenderSystem<IStandardRenderable, InstanceRenderSystem> RenderSystem => InstanceRenderSystem.Singleton;

        public Directions Direction => Directions.NONE;

        public StandardRenderableTurf(int x, int y) : base(x, y)
        {
            InstanceRenderSystem.Singleton.StartRendering(this);
        }

        public Model GetModel()
        {
            return QuadModelData.Singleton.model;
        }

        public Vector<float> GetPosition()
        {
            return new Vector<float>(X, Y, 0);
        }

        public RendererTextureData GetRendererTextureData()
        {
            return TextureCache.GetTexture(Texture);
        }

        public override bool Destroy()
        {
            InstanceRenderSystem.Singleton.StopRendering(this);
            return base.Destroy();
        }

        public uint GetTextureUint()
        {
            return GetRendererTextureData().TextureUint;
        }

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

        private Jetpack debugJetpack;

        public override void OnAtmosphereChanged(AtmosphericBlock block)
        {
            //DEBUG
            if (block != null && debugJetpack == null)
                debugJetpack = new Jetpack(new Vector<float>(X, Y));
            else if (block == null && debugJetpack != null)
                debugJetpack.Destroy();
        }
    }
}
