using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.Construction;
using GJ2022.Game.Construction.Walls;
using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GJ2022.Entities.Abstract
{
    public class Blueprint : Entity, IBlueprintRenderable, IDestroyable
    {

        private bool isDestroyed = false;

        public BlueprintDetail BlueprintDetail { get; set; } = new FoundationBlueprint();

        public RenderSystem<IBlueprintRenderable, BlueprintRenderSystem> RenderSystem => BlueprintRenderSystem.Singleton;

        public ModelData ModelData { get; set; } = QuadModelData.Singleton;

        private string usingTexture = "";

        public Type CreatedType { get; }

        public Blueprint(Vector position, string texture, Type createdType) : base(position)
        {
            //Set using texture
            usingTexture = texture;
            //Set the created type
            CreatedType = createdType;
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
            if (PawnControllerSystem.Singleton.QueuedBlueprints.ContainsKey(position))
            {
                PawnControllerSystem.Singleton.QueuedBlueprints[position][BlueprintDetail.BlueprintLayer] = null;
                bool exists = false;
                for (int i = 0; i < BlueprintDetail.MAX_BLUEPRINT_LAYER; i++)
                {
                    if (PawnControllerSystem.Singleton.QueuedBlueprints[position][i] != null)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    PawnControllerSystem.Singleton.QueuedBlueprints.Remove(position);
                }
            }
            return true;
        }

        public void Complete()
        {
            //Create an instance of the thingy
            Activator.CreateInstance(CreatedType, position);
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

        public Vector GetPosition()
        {
            return position;
        }

        public RendererTextureData GetRendererTextureData()
        {
            return TextureCache.GetTexture(usingTexture);
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
