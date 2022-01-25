using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GJ2022.Rendering.Text.TextObject;

namespace GJ2022.Rendering.RenderSystems.Interfaces
{
    public interface ITextRenderable : IInstanceRenderable<ITextRenderable, TextRenderSystem>
    {

        char Character { get; set; }
        Vector<float> Position { get; set; }        //The position of the text
        PositionModes PositionMode { get; set; }
        float Scale { get; set; }                   //The scale of the text object, larger values indicate larger text.
        float Width { get; set; }
        float Height { get; set; }
        Colour Colour { get; set; }                 //The colour of the text object.

    }
}
