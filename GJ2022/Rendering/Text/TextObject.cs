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
    public class TextObject
    {

        public enum PositionModes
        {
            SCREEN_POSITION = 0,
            WORLD_POSITION = 1,
        }

        private string _text = "";
        private Vector<float> _position;
        public Vector<float> Position
        {
            get { return _position; }
            set
            {
                _position = value;
                float currentXOffset = 0;
                float currentYOffset = 0;
                foreach (RenderableCharacter renderableChar in characters)
                {
                    TextCharacter textChar = TextLoader.textCharacters[renderableChar.Character];
                    renderableChar.Position = Position + new Vector<float>(currentXOffset, currentYOffset);
                    //Apply offset
                    currentXOffset += Scale * (textChar.width_offset + textChar.base_width) / 32.0f;
                    if (currentXOffset > Width)
                    {
                        currentXOffset = 0;
                        currentYOffset -= 0.3f;
                    }
                }
                if (isRendering)
                {
                    StopRendering();
                    StartRendering();
                }
            }
        }
        public PositionModes PositionMode { get; set; } = PositionModes.SCREEN_POSITION;  //The positioning mode of the text
        public float Scale { get; set; } = 1;                           //The scale of the text object, larger values indicate larger text.
        public float Width { get; set; } = 10;                          //The line width of the object, how many world units before text wraps to the next line.
        public Colour Colour { get; set; } = new Colour(1, 1, 1);       //The colour of the text object.

        private List<RenderableCharacter> characters = new List<RenderableCharacter>();

        private bool isRendering = true;

        public TextObject(
            string text,
            Colour colour,
            Vector<float> position,
            PositionModes positionMode = PositionModes.SCREEN_POSITION,
            float scale = 1,
            float width = 10)
        {
            Position = position;
            PositionMode = positionMode;
            Scale = scale;
            Width = width;
            Colour = colour;
            Text = text;
        }

        public string Text
        {
            get { return _text; }
            set {
                _text = value;
                if (isRendering)
                {
                    foreach (RenderableCharacter renderableChar in characters)
                    {
                        TextRenderSystem.Singleton.StopRendering(renderableChar);
                    }
                }
                characters.Clear();
                float currentXOffset = 0;
                float currentYOffset = 0;
                foreach (char character in _text)
                {
                    TextCharacter textChar = TextLoader.textCharacters[character];
                    //Create the character
                    RenderableCharacter createdCharacter = new RenderableCharacter(
                        character,
                        Position + new Vector<float>(currentXOffset, currentYOffset),
                        PositionMode,
                        Scale,
                        textChar.base_width,
                        32,
                        Colour
                    );
                    if (!isRendering)
                        TextRenderSystem.Singleton.StopRendering(createdCharacter);
                    characters.Add(createdCharacter);
                    //Apply offset
                    currentXOffset += Scale * (textChar.width_offset + textChar.base_width) / 32.0f;
                    if (currentXOffset > Width)
                    {
                        currentXOffset = 0;
                        currentYOffset -= 0.3f;
                    }
                }
            }
        }

        public void StartRendering()
        {
            if (isRendering)
                return;
            isRendering = true;
            foreach (RenderableCharacter renderableChar in characters)
            {
                TextRenderSystem.Singleton.StartRendering(renderableChar);
            }
        }

        public void StopRendering()
        {
            if (!isRendering)
                return;
            isRendering = false;
            foreach (RenderableCharacter renderableChar in characters)
            {
                TextRenderSystem.Singleton.StopRendering(renderableChar);
            }
        }

    }
}
