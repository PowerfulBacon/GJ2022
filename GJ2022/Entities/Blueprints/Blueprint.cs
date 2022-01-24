using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.Construction;
using GJ2022.Game.Construction.Blueprints;
using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GJ2022.Entities.Blueprints
{
    public class Blueprint : Entity, IBlueprintRenderable, IDestroyable
    {

        private bool isDestroyed = false;

        public BlueprintDetail BlueprintDetail { get; set; }

        public RenderSystem<IBlueprintRenderable, BlueprintRenderSystem> RenderSystem => BlueprintRenderSystem.Singleton;

        public ModelData ModelData { get; set; } = QuadModelData.Singleton;

        public Blueprint(Vector<float> position, BlueprintDetail blueprint) : base(position)
        {
            //Set the blueprint details
            BlueprintDetail = blueprint;
            //Update batch
            if (renderableBatchIndex.Count > 0)
                (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IBlueprintRenderable, BlueprintRenderSystem>)?.UpdateBatchData(this, 1);
        }

        public bool Destroy()
        {
            //Set destroyed
            isDestroyed = true;
            //Stop rendering
            BlueprintRenderSystem.Singleton.StopRendering(this);
            //Remove from the pawn list
            //TODO: Contain this inside pawn controller system rather than here
            if (PawnControllerSystem.QueuedBlueprints.ContainsKey(Position))
            {
                if (PawnControllerSystem.QueuedBlueprints[Position][BlueprintDetail.BlueprintLayer] == this)
                    PawnControllerSystem.QueuedBlueprints[Position].Remove(BlueprintDetail.BlueprintLayer);
                if (PawnControllerSystem.QueuedBlueprints[Position].Count == 0)
                    PawnControllerSystem.QueuedBlueprints.Remove(Position);
            }
            return true;
        }

        public virtual void Complete()
        {
            //Create an instance of the thingy
            Activator.CreateInstance(BlueprintDetail.CreatedType, Position);
            //Destroy the blueprint
            Destroy();
        }

        public bool IsDestroyed()
        {
            return isDestroyed;
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
            return new Vector<float>(Position[0], Position[1], 2);
        }

        public RendererTextureData GetRendererTextureData()
        {
            return TextureCache.GetTexture(BlueprintDetail.Texture);
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

    }
}
