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
    public class UserInterfaceRenderSystem : RenderSystem<IUserInterfaceRenderable, UserInterfaceRenderSystem>
    {

        public static UserInterfaceRenderSystem Singleton;

        protected override string SystemShaderName => "userInterfaceShader";

        protected override int BufferCount => 3;

        protected override int[] BufferWidths => new int[] { 4, 4, 2 };

        protected override uint[] BufferDataPointsPerInstance => new uint[] { 1, 1, 1 };

        public override void EndRender()
        {
            return;
        }

        public override float[] GetBufferData(IUserInterfaceRenderable targetItem, int bufferIndex)
        {
            switch (bufferIndex)
            {
                case 0:
                    Vector<float> position = targetItem.Position;
                    return new float[] {
                        position[0],
                        position[1],
                        targetItem.Layer,
                        (float)targetItem.PositionMode
                    };
                case 1:
                    RendererTextureData texData = targetItem.GetRendererTextureData();
                    return new float[] {
                        texData.IndexX,
                        texData.IndexY,
                        texData.Width,
                        texData.Height,
                    };
                case 2:
                    Vector<float> scale = targetItem.Scale;
                    return new float[] {
                        scale[0],
                        scale[1]
                    };
            }
            throw new ArgumentException($"Invalid argument buffer index supplied: buffer supplied {bufferIndex}, maxBuffer: {BufferCount}");
        }

        protected override RenderBatchGroup GetBatchGroup(IUserInterfaceRenderable renderable)
        {
            return new RenderBatchGroup(QuadModelData.Singleton.model, renderable.GetRendererTextureData().TextureUint);
        }

        protected override void SetSingleton()
        {
            Singleton = this;
        }

    }
}
