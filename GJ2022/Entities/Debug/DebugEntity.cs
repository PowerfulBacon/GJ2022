using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.ComponentInterfaces.MouseEvents;
using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Debug
{

    /// <summary>
    /// Debug entity, add whatever you want to this for testing.
    /// </summary>
    public class DebugEntity : Entity, IInstanceRenderable, IMouseEnter, IMouseExit, IDestroyable
    {

        public override ModelData ModelData { get; set; } = QuadModelData.Singleton;

        public float WorldX => position[0] - 0.5f;

        public float WorldY => position[1] - 0.5f;

        public float Width => 1.0f;

        public float Height => 1.0f;

        public DebugEntity(Vector position) : base(position)
        {
            MouseCollisionSubsystem.Singleton.StartTracking(this);
        }

        public override RendererTextureData GetRendererTexture()
        {
            return TextureCache.GetTexture(TextureCache.ERROR_ICON_STATE);
        }

        /// <summary>
        /// Instance renderable stuff
        /// </summary>

        private Dictionary<RenderBatchSet, int> renderableBatchIndex = new Dictionary<RenderBatchSet, int>();

        public void SetRenderableBatchIndex(RenderBatchSet associatedSet, int index)
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
        public int GetRenderableBatchIndex(RenderBatchSet associatedSet)
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
    }
}
