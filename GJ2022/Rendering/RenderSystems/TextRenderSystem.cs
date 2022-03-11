using GJ2022.Game.GameWorld;
using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using System;
using static OpenGL.Gl;

namespace GJ2022.Rendering.RenderSystems
{
    public class TextRenderSystem : RenderSystem<ITextRenderable, TextRenderSystem>
    {

        public static TextRenderSystem Singleton;

        protected override string SystemShaderName => "textShader";

        protected override int BufferCount => 4;

        protected override int[] BufferWidths { get; } = new int[] { 4, 4, 4, 2 };

        protected override uint[] BufferDataPointsPerInstance { get; } = new uint[] { 1, 1, 1, 1 };

        protected override string[] UniformVariableNames => new string[] {
            "viewMatrix",
            "projectionMatrix",
            "textureSampler",
        };

        protected override RenderBatchGroup GetBatchGroup(ITextRenderable renderable)
        {
            return new RenderBatchGroup(QuadModelData.Singleton.model, renderable.GetRendererTextureData().TextureUint);
        }

        protected override void SetSingleton()
        {
            Singleton = this;
        }

        public override void EndRender()
        {
            return;
        }

        protected override unsafe void BindUniformData(RenderBatchSet<ITextRenderable, TextRenderSystem> renderBatchSet)
        {
            glUniform1i(uniformVariableLocations["textureSampler"], 0);
        }

        public override float[] GetBufferData(ITextRenderable targetItem, int bufferIndex)
        {
            switch (bufferIndex)
            {
                case 0:
                    Vector<float> position = targetItem.Position;
                    return new float[] {
                        position.X,
                        position.Y,
                        Layers.LAYER_TEXT,
                        (float)targetItem.PositionMode
                    };
                case 1:
                    RendererTextureData texData = targetItem.GetRendererTextureData();
                    return new float[] {
                        targetItem.Character,
                        targetItem.Width,
                        targetItem.Height,
                        texData.FileWidth,
                    };
                case 2:
                    Colour colour = targetItem.Colour;
                    return new float[] {
                        colour.red,
                        colour.green,
                        colour.blue,
                        colour.alpha
                    };
                case 3:
                    return new float[] {
                        targetItem.Scale,
                        0.0f,
                    };
            }
            throw new ArgumentException($"Invalid argument buffer index supplied: buffer supplied {bufferIndex}, maxBuffer: {BufferCount}");
        }
    }
}
