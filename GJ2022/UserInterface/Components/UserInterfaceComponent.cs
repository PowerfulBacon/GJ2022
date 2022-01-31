using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.UserInterface.Components
{
    public abstract class UserInterfaceComponent
    {

        public enum PositionModes
        {
            SCREEN_POSITION = 0,
            WORLD_POSITION = 1,
        }

        public virtual UserInterfaceComponent Parent { get; set; }

        //The renderable attached to thie UI component.
        public abstract Renderable Renderable { get; }

        //Positional stuff
        public abstract Vector<float> Position { get; set; }

        //Scale getter
        public abstract Vector<float> Scale { get; }

        //Positional mode
        public PositionModes PositionMode { get; set; } = PositionModes.SCREEN_POSITION;

        public abstract void Hide();

        public abstract void Show();

    }
}
