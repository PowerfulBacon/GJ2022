using GJ2022.Rendering.Models;
using GJ2022.UserInterface.Components;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Rendering.RenderSystems.Interfaces
{
    public interface IButtonRenderable : IInstanceRenderable<IButtonRenderable, ButtonRenderSystem>
    {

        Model Model { get; }

        Vector<float> Position { get; }

        Vector<float> Scale { get; }

        float Layer { get; }

        UserInterfaceComponent.PositionModes PositionMode { get; }

        Colour Colour { get; }

        bool isHovered { get; }

    }
}
