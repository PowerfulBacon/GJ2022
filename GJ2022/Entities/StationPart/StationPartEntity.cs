using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.Entities.StationPart
{
    public abstract class StationPartEntity : Entity, IStandardRenderable
    {

        public override ModelData ModelData { get; set; } = QuadModelData.Singleton;

        //List of rooms connected to this one
        public List<StationPartEntity> AttachedRooms { get; } = new List<StationPartEntity>();

        //Set the relative connection points
        public virtual Vector[] RelativeConnectionPoints { get; protected set; }

        public RenderSystem<IStandardRenderable, InstanceRenderSystem> RenderSystem => InstanceRenderSystem.Singleton;

        //Ctor
        public StationPartEntity(Vector position) : base(position) { }

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

        public Vector GetInstancePosition()
        {
            return position;
        }

        public Vector GetInstanceScale()
        {
            return new Vector(3, 1);
        }

        public Model GetModel()
        {
            return ModelData.model;
        }

        public uint GetTextureUint()
        {
            return GetRendererTextureData().TextureUint;
        }

        public Vector GetPosition()
        {
            return position;
        }

        public RendererTextureData GetRendererTextureData()
        {
            return TextureCache.GetTexture(TextureCache.ERROR_ICON_STATE);
        }
    }
}
