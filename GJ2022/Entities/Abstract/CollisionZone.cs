using GJ2022.Entities.ComponentInterfaces.MouseEvents;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Abstract
{
    public class CollisionZone : IMouseEnter, IMouseExit
    {

        public delegate void OnMouseDelegate();

        public OnMouseDelegate onMouseEnter = null;
        public OnMouseDelegate onMouseExit = null;

        public float WorldX { get; private set; }

        public float WorldY { get; private set; }

        public float Width { get; private set; }

        public float Height { get; private set; }

        public CursorSpace PositionSpace => CursorSpace.WORLD_SPACE;

        public CollisionZone(Vector<float> position, Vector<float> scale, OnMouseDelegate mouseEnterEvent = null, OnMouseDelegate mouseExitEvent = null)
        {
            WorldX = position[0];
            WorldY = position[1];
            Width = scale[0];
            Height = scale[1];
            onMouseEnter = mouseEnterEvent;
            onMouseExit = mouseExitEvent;
        }

        public void OnMouseEnter()
        {
            onMouseEnter?.Invoke();
        }

        public void OnMouseExit()
        {
            onMouseExit?.Invoke();
        }
    }
}
