using GJ2022.Entities.ComponentInterfaces.MouseEvents;
using GJ2022.GlobalDataComponents;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;
using GLFW;
using System.Collections.Generic;
using System.Linq;

namespace GJ2022.Subsystems
{

    /// <summary>
    /// Handles mouse entering events
    /// </summary>
    public class MouseCollisionSubsystem : Subsystem
    {

        private enum MouseCollisionState
        {
            NONE = 0,
            MOUSE_OVER = 1 << 0,
            MOUSE_CLICK = 1 << 1,
        };

        public static MouseCollisionSubsystem Singleton { get; } = new MouseCollisionSubsystem();

        //50 FPS
        public override int sleepDelay => 20;

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_PROCESSING;

        //Tracking mouse enter events
        private Dictionary<IMouseEvent, MouseCollisionState> trackingEvents = new Dictionary<IMouseEvent, MouseCollisionState>();

        public void StartTracking(IMouseEvent tracker)
        {
            trackingEvents.Add(tracker, MouseCollisionState.NONE);
        }

        public void StopTracking(IMouseEvent tracker)
        {
            trackingEvents.Remove(tracker);
        }

        public override void Fire(Window window)
        {
            //Fetch position of the mouse
            double cursorX;
            double cursorY;
            Glfw.GetCursorPosition(window, out cursorX, out cursorY);
            //Get the size of the window
            int windowWidth;
            int windowHeight;
            Glfw.GetWindowSize(window, out windowWidth, out windowHeight);
            //Convert to world coords
            Vector<float> worldCoordinates = ScreenToWorldHelper.GetWorldCoordinates(new Vector<float>((float)cursorX, (float)cursorY), new Vector<float>(windowWidth, windowHeight));
            Vector<float> screenCoordinates = new Vector<float>((float)cursorX / windowWidth * 2.0f - 1, (float)cursorY / windowHeight * 2.0f - 1);
            //Log.WriteLine(screenCoordinates);
            bool mousePressed = Glfw.GetMouseButton(window, MouseButton.Left) == InputState.Press;
            //Go through all tracking events and handle them
            for (int i = trackingEvents.Count - 1; i >= 0; i--)
            {
                IMouseEvent mouseEventHolder = trackingEvents.Keys.ElementAt(i);
                MouseCollisionState collisionState = trackingEvents[mouseEventHolder];
                //Calculate the world coordinates
                double minimumX = mouseEventHolder.WorldX;
                double maximumX = minimumX + mouseEventHolder.Width;
                double minimumY = mouseEventHolder.WorldY;
                double maximumY = minimumY + mouseEventHolder.Height;
                Vector<float> coordinates = mouseEventHolder.PositionSpace == CursorSpace.SCREEN_SPACE ? screenCoordinates : worldCoordinates;
                //Check if colliding
                bool colliding = coordinates[0] >= minimumX
                    && coordinates[0] <= maximumX
                    && coordinates[1] >= minimumY
                    && coordinates[1] <= maximumY;
                switch (collisionState & MouseCollisionState.MOUSE_OVER)
                {
                    case MouseCollisionState.NONE:
                        if (colliding && mouseEventHolder is IMouseEnter)
                            (mouseEventHolder as IMouseEnter).OnMouseEnter();
                        break;
                    case MouseCollisionState.MOUSE_OVER:
                        if (!colliding && mouseEventHolder is IMouseExit)
                            (mouseEventHolder as IMouseExit).OnMouseExit();
                        if (colliding)
                        {
                            if ((collisionState & MouseCollisionState.MOUSE_CLICK) == 0)
                            {
                                if (mousePressed)
                                {
                                    collisionState |= MouseCollisionState.MOUSE_CLICK;
                                }
                            }
                            else if (!mousePressed)
                            {
                                (mouseEventHolder as IMousePress)?.OnPressed();
                                collisionState &= ~MouseCollisionState.MOUSE_CLICK;
                            }
                        }
                        break;
                }
                //Set new collision state
                if (colliding)
                    collisionState |= MouseCollisionState.MOUSE_OVER;
                else
                    collisionState &= ~MouseCollisionState.MOUSE_OVER;
                trackingEvents[mouseEventHolder] = collisionState;
            }
        }

        public override void InitSystem()
        { }

        protected override void AfterWorldInit()
        { }

    }
}
