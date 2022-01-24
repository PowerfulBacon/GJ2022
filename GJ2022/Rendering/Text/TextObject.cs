﻿using GJ2022.Rendering.RenderSystems;
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
            SCREEN_POSITION,
            WORLD_POSITION
        }

        private string _text = "";
        public Vector<float> Position { get; set; }                     //The position of the text
        public PositionModes PositionMode { get; set; } = PositionModes.SCREEN_POSITION;  //The positioning mode of the text
        public float Scale { get; set; } = 1;                           //The scale of the text object, larger values indicate larger text.
        public float Width { get; set; } = 10;                          //The line width of the object, how many world units before text wraps to the next line.
        public Colour Colour { get; set; } = new Colour(1, 1, 1);       //The colour of the text object.

        private List<RenderableCharacter> characters = new List<RenderableCharacter>();

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
                foreach (RenderableCharacter renderableChar in characters)
                {
                    TextRenderSystem.Singleton.StopRendering(renderableChar);
                }
                characters.Clear();
                float currentXOffset = 0;
                float currentYOffset = 0;
                foreach (char character in _text)
                {
                    TextCharacter textChar = TextLoader.textCharacters[character];
                    //Create the character
                    characters.Add(new RenderableCharacter(
                        character,
                        Position + new Vector<float>(currentXOffset, currentYOffset),
                        PositionMode,
                        Scale,
                        textChar.base_width,
                        32,
                        Colour
                    ));
                    //Apply offset
                    currentXOffset += textChar.width_offset;
                    if (currentXOffset > Width)
                    {
                        currentXOffset = 0;
                        currentYOffset += 32;
                    }
                }
            }
        }

    }
}
