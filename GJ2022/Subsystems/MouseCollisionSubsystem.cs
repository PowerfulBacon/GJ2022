using GJ2022.Entities.ComponentInterfaces.MouseEvents;
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
            NONE,
            MOUSE_OVER
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
            Vector worldCoordinates = ScreenToWorldHelper.GetWorldCoordinates(new Vector(2, (float)cursorX, (float)cursorY), new Vector(2, windowWidth, windowHeight));
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
                //Check if colliding
                bool colliding = worldCoordinates[0] >= minimumX
                    && worldCoordinates[0] <= maximumX
                    && worldCoordinates[1] >= minimumY
                    && worldCoordinates[1] <= maximumY;
                switch (collisionState)
                {
                    case MouseCollisionState.NONE:
                        if (colliding && mouseEventHolder is IMouseEnter)
                            (mouseEventHolder as IMouseEnter).OnMouseEnter();
                        break;
                    case MouseCollisionState.MOUSE_OVER:
                        if (!colliding && mouseEventHolder is IMouseExit)
                            (mouseEventHolder as IMouseExit).OnMouseExit();
                        break;
                }
                //Set new collision state
                collisionState = colliding ? MouseCollisionState.MOUSE_OVER : MouseCollisionState.NONE;
                trackingEvents[mouseEventHolder] = collisionState;
            }
        }

        public override void InitSystem()
        { }

        protected override void AfterWorldInit()
        { }

    }
}
