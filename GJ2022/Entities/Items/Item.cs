using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items
{
    public abstract class Item : Entity, IStandardRenderable, IDestroyable, IMovable
    {

        //Location of the item, null if the item is on the ground
        public Entity Location { get; private set; }

        public abstract string Texture { get; }

        private bool isDestroyed = false;

        public RenderSystem<IStandardRenderable, InstanceRenderSystem> RenderSystem => InstanceRenderSystem.Singleton;

        public Item(Vector<float> position) : base(position)
        {
            RenderSystem.StartRendering(this);
        }

        public bool Destroy()
        {
            if(Location == null)
                InstanceRenderSystem.Singleton.StopRendering(this);
            isDestroyed = true;
            return true;
        }

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

        public bool IsDestroyed()
        {
            return isDestroyed;
        }

        public void UpdatePositionBatch()
        {
            //Update position in renderer
            if (renderableBatchIndex.Count > 0)
                (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IStandardRenderable, InstanceRenderSystem>)?.UpdateBatchData(this, 0);
        }

        public virtual void OnMoved(Vector<float> previousPosition)
        {
            return;
        }
    }
}
