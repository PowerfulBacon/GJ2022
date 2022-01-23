﻿using GJ2022.Entities.Abstract;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Subsystems;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns
{
    public class Pawn : Entity, ICircleRenderable, IProcessable
    {

        public Pawn(Vector position) : base(position)
        {
            PawnControllerSystem.Singleton.StartProcessing(this);
            CircleRenderSystem.Singleton.StartRendering(this);
        }

        public Colour Colour { get; } = Colour.Yellow;

        public RenderSystem<ICircleRenderable, CircleRenderSystem> RenderSystem => CircleRenderSystem.Singleton;

        private Blueprint workTarget;

        public void Process(float deltaTime)
        {
            //Target no longer exists
            if (workTarget != null && workTarget.IsDestroyed())
                workTarget = null;
            //Move towards the nearest blueprint and build it
            if (workTarget == null)
            {
                if (PawnControllerSystem.Singleton.QueuedBlueprints.Count > 0)
                {
                    Vector workTargetPosition = ListPicker.Pick(PawnControllerSystem.Singleton.QueuedBlueprints.Keys);
                    Blueprint[] blueprintTarget = PawnControllerSystem.Singleton.QueuedBlueprints[workTargetPosition];
                    if (blueprintTarget[0] != null)
                        workTarget = blueprintTarget[0];
                    else
                        workTarget = blueprintTarget[1];
                }
                else
                    return;
            }
            //Move towards
            position.MoveTowards(workTarget.position, 0.1f, deltaTime);
            //Update position in renderer
            if (renderableBatchIndex.Count > 0)
                (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<ICircleRenderable, CircleRenderSystem>)?.UpdateBatchData(this, 0);
            //If distance < build range, build it
            if (position.IgnoreZ() == workTarget.position.IgnoreZ())
            {
                workTarget.Complete();
            }
        }

        public Vector GetPosition()
        {
            return position;
        }

        public RendererTextureData GetRendererTextureData()
        {
            return TextureCache.GetTexture(TextureCache.ERROR_ICON_STATE);
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

        private bool destroyed = false;

        public bool Destroy()
        {
            CircleRenderSystem.Singleton.StopRendering(this);
            PawnControllerSystem.Singleton.StopProcessing(this);
            destroyed = true;
            return true;
        }

        public bool IsDestroyed()
        {
            return destroyed;
        }
    }
}
