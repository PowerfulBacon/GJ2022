using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Utility.MathConstructs;
using System;

namespace GJ2022.Rendering.RenderSystems
{
    public class CircleRenderSystem : RenderSystem<ICircleRenderable, CircleRenderSystem>
    {

        public static CircleRenderSystem Singleton;

        protected override string SystemShaderName => "circleShader";

        protected override int BufferCount => 2;

        protected override int[] BufferWidths { get; } = new int[] { 3, 3 };

        protected override uint[] BufferDataPointsPerInstance { get; } = new uint[] { 1, 1 };

        protected override RenderBatchGroup GetBatchGroup(ICircleRenderable renderable)
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

        public override float[] GetBufferData(ICircleRenderable targetItem, int bufferIndex)
        {
            switch (bufferIndex)
            {
                case 0:
                    Vector<float> position = targetItem.GetPosition();
                    return new float[] {
                        position.X,
                        position.Y,
                        position.Z
                    };
                case 1:
                    return new float[] {
                        targetItem.Colour.red,
                        targetItem.Colour.green,
                        targetItem.Colour.blue,
                    };
            }
            throw new ArgumentException($"Invalid argument buffer index supplied: buffer supplied {bufferIndex}, maxBuffer: {BufferCount}");
        }
    }
}
