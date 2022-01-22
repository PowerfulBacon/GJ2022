using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.RenderSystems.RenderData;
using System;
using static OpenGL.Gl;

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

        protected override bool[] IsInstanceBuffer { get; } = new bool[] { true, true };

        protected override RenderBatchGroup GetBatchGroup(IStandardRenderable renderable)
        {
            return new RenderBatchGroup(renderable.GetModel(), renderable.GetTextureUint());
        }

        protected override void SetSingleton()
        {
            Singleton = this;
        }

        public override unsafe float* GetBufferPointer(RenderBatch<IStandardRenderable, InstanceRenderSystem> targetBatch, int index)
        {
            switch (index)
            {
                case 2:
                    return &targetBatch.bufferArrays[0];
            }
            return null;
        }

        public override void EndRender()
        {
            throw new NotImplementedException();
        }

        protected override uint GetUnmanagedBufferLocation(uint target, RenderBatchGroup targetBatch)
        {
            switch (target)
            {
                case 0:
                    return targetBatch.Model.VertexBufferObject;
                case 1:
                    return targetBatch.Model.UvBuffer;
            }
            return 0;
        }
    }
}
