using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items;
using GJ2022.Entities.Items.Stacks;
using GJ2022.Game.Construction.Blueprints;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Subsystems;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;

namespace GJ2022.Entities.Blueprints
{
    public class Blueprint : Entity, IDestroyable
    {
        //Materials loaded into this blueprint
        public Dictionary<Type, int> LoadedMaterials { get; } = new Dictionary<Type, int>();

        public BlueprintDetail BlueprintDetail { get; set; }

        public RenderSystem<IBlueprintRenderable, BlueprintRenderSystem> RenderSystem => BlueprintRenderSystem.Singleton;

        public ModelData ModelData { get; set; } = QuadModelData.Singleton;

        public bool Destroyed { get; set; } = false;

        protected override Renderable Renderable { get; set; } = new BlueprintRenderable("error");

        protected List<Item> contents = new List<Item>();

        public Blueprint(Vector<float> position, BlueprintDetail blueprint) : base(position, Layers.LAYER_BLUEPRINT)
        {
            //Set the blueprint details
            BlueprintDetail = blueprint;
            //Update the texture
            Texture = BlueprintDetail.Texture;
        }

        public override bool Destroy()
        {
            base.Destroy();
            //Set destroyed
            Destroyed = true;
            //Drop our contents
            foreach (Item item in contents)
            {
                item.Location = null;
                item.Position = Position.Copy();
            }
            //Remove from the pawn list
            //TODO: Contain this inside pawn controller system rather than here
            if (PawnControllerSystem.QueuedBlueprints.ContainsKey(Position))
            {
                if (PawnControllerSystem.QueuedBlueprints[Position].ContainsKey(BlueprintDetail.BlueprintLayer)
                        && PawnControllerSystem.QueuedBlueprints[Position][BlueprintDetail.BlueprintLayer] == this)
                    PawnControllerSystem.QueuedBlueprints[Position].Remove(BlueprintDetail.BlueprintLayer);
                if (PawnControllerSystem.QueuedBlueprints[Position].Count == 0)
                    PawnControllerSystem.QueuedBlueprints.Remove(Position);
            }
            return true;
        }

        public void PutMaterials(Item item)
        {
            if (!(item is Stack stackItem))
            {
                item.Location = this;
                LazyHelper.LazyIntegerAdd(LoadedMaterials, item.GetType(), item.Count());
            }
            else
            {
                Stack toPut = stackItem.Take(Math.Min(stackItem.StackSize, BlueprintDetail.Cost.Cost[item.GetType()]));
                toPut.Location = this;
                LazyHelper.LazyIntegerAdd(LoadedMaterials, item.GetType(), toPut.StackSize);
            }
        }

        public bool RequiresMaterial(Type materialType)
        {
            return BlueprintDetail.Cost.Cost.ContainsKey(materialType);
        }

        /// <summary>
        /// Returns a tuple containing the first required material type and the amount needed
        /// </summary>
        public (Type, int)? GetRequiredMaterial()
        {
            foreach (Type requiredType in BlueprintDetail.Cost.Cost.Keys)
            {
                int requiredAmount = BlueprintDetail.Cost.Cost[requiredType];
                if (!LoadedMaterials.ContainsKey(requiredType))
                {
                    return (requiredType, requiredAmount);
                }
                if (LoadedMaterials[requiredType] < requiredAmount)
                {
                    return (requiredType, requiredAmount - LoadedMaterials[requiredType]);
                }
            }
            return null;
        }

        /// <summary>
        /// Returns true if the blueprint has all the materials needed loaded
        /// </summary>
        public bool HasMaterials()
        {
            return GetRequiredMaterial() == null;
        }

        public virtual void Complete()
        {
            //Clear and delete all contents
            foreach (Item item in contents)
            {
                item.Destroy();
            }
            contents.Clear();
            //Create an instance of the thingy
            Activator.CreateInstance(BlueprintDetail.CreatedType, Position);
            //Destroy the blueprint
            Destroy();
        }

    }
}
