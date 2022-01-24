using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Rendering.Text
{
    class RenderableCharacter : ITextRenderable
    {

        public RenderSystem<ITextRenderable, TextRenderSystem> RenderSystem => TextRenderSystem.Singleton;

        public char Character { get; set; }
        public Vector<float> Position { get; set; }
        public TextObject.PositionModes PositionMode { get; set; }
        public float Scale { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Colour Colour { get; set; }

        protected Dictionary<object, int> renderableBatchIndex = new Dictionary<object, int>();

        public RenderableCharacter(char character, Vector<float> position, TextObject.PositionModes positionMode, float scale, float width, float height, Colour colour)
        {
            Character = character;
            Position = position;
            PositionMode = positionMode;
            Scale = scale;
            Width = width;
            Height = height;
            Colour = colour;
            TextRenderSystem.Singleton.StartRendering(this);
        }

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

        public RendererTextureData GetRendererTextureData()
        {
            return TextureCache.GetTexture("text");
        }
    }
}
