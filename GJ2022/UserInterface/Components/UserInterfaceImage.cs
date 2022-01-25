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

        public override Vector<float> Scale => (Renderable as UserInterfaceRenderable).Scale;

        public UserInterfaceImage(Vector<float> position, Vector<float> scale, string icon)
        {
            Renderable = new UserInterfaceRenderable(position, PositionMode, scale, icon);
            _position = position;
        }

        public override void Hide()
        {
            Renderable.StopRendering();
        }

        public override void Show()
        {
            Renderable.StartRendering();
        }
    }
}
