using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.UserInterface.Components
{
    class UserInterfaceImage : UserInterfaceComponent
    {

        public override Renderable Renderable { get; }

        private Vector<float> _position;
        public override Vector<float> Position
        {
            get { return _position; }
            set {
                _position = value;
                Renderable?.moveHandler?.Invoke(value);
            }
        }

        public UserInterfaceImage(Vector<float> position, string icon)
        {
            Renderable = new UserInterfaceRenderable(position, PositionMode, new Vector<float>(2.0f, 0.3f), icon);
            _position = position;
        }

    }
}
