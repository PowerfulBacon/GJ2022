using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.ComponentInterfaces.MouseEvents;
using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.Entities.Debug
{

    /// <summary>
    /// Debug entity, add whatever you want to this for testing.
    /// </summary>
    public class DebugEntity : Entity, IStandardRenderable, IMouseEnter, IMouseExit, IDestroyable
    {

        public ModelData ModelData { get; set; } = QuadModelData.Singleton;

        public float WorldX => position[0] - 0.5f;

        public float WorldY => position[1] - 0.5f;

        public float Width => 1.0f;

        public float Height => 1.0f;

        public RenderSystem<IStandardRenderable, InstanceRenderSystem> RenderSystem => InstanceRenderSystem.Singleton;

        protected virtual string Texture { get; set; } = TextureCache.ERROR_ICON_STATE;

        public DebugEntity(Vector<float> position) : base(position)
        {
            MouseCollisionSubsystem.Singleton.StartTracking(this);
            InstanceRenderSystem.Singleton.StartRendering(this);
        }

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

        public Vector<float> GetInstancePosition()
        {
            return position;
        }

        public void OnMouseEnter()
        {
            Log.WriteLine("Mouse entered!");
        }

        public void OnMouseExit()
        {
            Log.WriteLine("Mouse exitted!");
        }

        public bool Destroy()
        {
            return false;
        }

        public bool IsDestroyed()
        {
            return false;
        }

        public Vector<float> GetInstanceScale()
        {
            return new Vector<float>(1, 1);
        }

        public Model GetModel()
        {
            return ModelData.model;
        }

        public uint GetTextureUint()
        {
            return GetRendererTextureData().TextureUint;
        }

        public Vector<float> GetPosition()
        {
            return position;
        }

        public RendererTextureData GetRendererTextureData()
        {
            return TextureCache.GetTexture(Texture);
        }
    }
}
