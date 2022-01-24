using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items
{
    public abstract class Item : Entity, IStandardRenderable
    {

        //Location of the item, null if the item is on the ground
        public Entity Location { get; private set; }

        public abstract string Texture { get; }

        public RenderSystem<IStandardRenderable, InstanceRenderSystem> RenderSystem => InstanceRenderSystem.Singleton;

        public Item(Vector<float> position) : base(position)
        { }

        //Put the item inside another entity
        public void PutInside(Entity target)
        {
            //Determine if we should be rendering
            if (target == null)
                if (Location != null)
                {
                    RenderSystem.StartRendering(this);
                    Position = Location.Position;
                }
            else if (Location == null)
                RenderSystem.StopRendering(this);
            //Set our location
            Location = target;
        }

        //==========================
        //Rendering zone
        //The unclean area
        //==========================

        public Model GetModel() => QuadModelData.Singleton.model;

        public uint GetTextureUint() => GetRendererTextureData().TextureUint;

        public RendererTextureData GetRendererTextureData() => TextureCache.GetTexture(Texture);

        public Vector<float> GetPosition() => Position;

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

    }
}
