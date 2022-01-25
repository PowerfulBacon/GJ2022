using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Rendering.RenderSystems
{
    public class ButtonRenderSystem : RenderSystem<IButtonRenderable, ButtonRenderSystem>
    {

        public static ButtonRenderSystem Singleton;

        protected override string SystemShaderName => "buttonShader";

        protected override int BufferCount => 4;

        protected override int[] BufferWidths => new int[] { 4, 2, 1, 4 };

        protected override uint[] BufferDataPointsPerInstance => new uint[] { 1, 1, 1, 1 };

        public override void EndRender()
        {
            return;
        }

        public override float[] GetBufferData(IButtonRenderable targetItem, int bufferIndex)
        {
            switch (bufferIndex)
            {
                case 0:
                    Vector<float> position = targetItem.Position;
                    return new float[] {
                        position[0],
                        position[1],
                        targetItem.Layer,
                        (float)targetItem.PositionMode,
                    };
                case 1:
                    Vector<float> scale = targetItem.Scale;
                    return new float[] {
                        scale[0],
                        scale[1]
                    };
                case 2:
                    return new float[] {
                        targetItem.isHovered ? 1.0f : 0.0f
                    };
                case 3:
                    return new float[] {
                        targetItem.Colour.red,
                        targetItem.Colour.green,
                        targetItem.Colour.blue,
                        targetItem.Colour.alpha,
                    };
            }
            throw new ArgumentException($"Invalid argument buffer index supplied: buffer supplied {bufferIndex}, maxBuffer: {BufferCount}");
        }

        protected override RenderBatchGroup GetBatchGroup(IButtonRenderable renderable)
        {
            return new RenderBatchGroup(QuadModelData.Singleton.model, renderable.GetRendererTextureData().TextureUint);
        }

        protected override void SetSingleton()
        {
            Singleton = this;
        }

    }
}
