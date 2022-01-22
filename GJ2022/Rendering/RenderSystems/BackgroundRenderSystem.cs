using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Rendering.RenderSystems
{
    class BackgroundRenderSystem : RenderSystem<IBackgroundRenderable, BackgroundRenderSystem>
    {

        public static BackgroundRenderSystem Singleton;

        protected override string SystemShaderName => "backgroundShader";

        protected override int BufferCount => 0;

        protected override int[] BufferWidths { get; } = new int[] { };

        protected override uint[] BufferDataPointsPerInstance { get; } = new uint[] { };

        protected override string[] UniformVariableNames => new string[] {
            "viewMatrix",
            "projectionMatrix",
        };

        protected override unsafe void BindUniformData(RenderBatchSet<IBackgroundRenderable, BackgroundRenderSystem> renderBatchSet)
        { }

        protected override RenderBatchGroup GetBatchGroup(IBackgroundRenderable renderable)
        {
            return new RenderBatchGroup(QuadModelData.Singleton.model, 0);
        }

        protected override void SetSingleton()
        {
            Singleton = this;
        }

        public override void EndRender()
        {
            return;
        }

        public override float[] GetBufferData(IBackgroundRenderable targetItem, int bufferIndex)
        {
            throw new NotImplementedException();
        }
    }
}
