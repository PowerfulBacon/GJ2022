﻿using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using System;

namespace GJ2022.Rendering.RenderSystems
{
    /// <summary>
    /// Rendering system optimised for batch rendering
    /// tons of the same object.
    /// </summary>
    public class InstanceRenderSystem : RenderSystem<IStandardRenderable, InstanceRenderSystem>
    {

        public static InstanceRenderSystem Singleton;

        protected override string SystemShaderName => "instanceShader";

        protected override int BufferCount => 2;

        protected override int[] BufferWidths { get; } = new int[] { 3, 4 };

        protected override uint[] BufferDataPointsPerInstance { get; } = new uint[] { 1, 1 };

        protected override RenderBatchGroup GetBatchGroup(IStandardRenderable renderable)
        {
            return new RenderBatchGroup(renderable.GetModel(), renderable.GetTextureUint());
        }

        protected override void SetSingleton()
        {
            Singleton = this;
        }

        public override void EndRender()
        {
            return;
        }

        public override float[] GetBufferData(IStandardRenderable targetItem, int bufferIndex)
        {
            switch (bufferIndex)
            {
                case 0:
                    Vector<float> position = targetItem.GetPosition();
                    return new float[] {
                        position[0],
                        position[1],
                        position[2]
                    };
                case 1:
                    RendererTextureData texData = targetItem.GetRendererTextureData();
                    return new float[] {
                        texData.IndexX,
                        texData.IndexY,
                        texData.Width,
                        texData.Height,
                    };
            }
            throw new ArgumentException($"Invalid argument buffer index supplied: buffer supplied {bufferIndex}, maxBuffer: {BufferCount}");
        }
    }
}
