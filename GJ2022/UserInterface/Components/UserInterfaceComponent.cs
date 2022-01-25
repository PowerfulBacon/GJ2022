using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.UserInterface.Components
{
    public abstract class UserInterfaceComponent
    {

        public enum PositionModes
        {
            SCREEN_POSITION = 0,
            WORLD_POSITION = 1,
        }

        //The renderable attached to thie UI component.
        public abstract Renderable Renderable { get; }

        //Positional stuff
        public abstract Vector<float> Position { get; set; }

        //Positional mode
        public PositionModes PositionMode { get; set; } = PositionModes.SCREEN_POSITION;

    }
}
