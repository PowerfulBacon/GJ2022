using GJ2022.Entities.ComponentInterfaces.MouseEvents;
using GJ2022.Rendering;
using GJ2022.Utility.MathConstructs;
using GLFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenGL.Gl;

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
            //Calculate the proportional position of the mouse
            cursorX /= windowWidth;
            cursorY /= windowHeight;
            //Convert from bounds [0, 1] to [-1, 1]
            cursorX = cursorX * 2 - 1;
            cursorY = cursorY * 2 - 1;
            //Guess
            Matrix cameraMatrix = RenderMaster.mainCamera.ProjectionMatrix * RenderMaster.mainCamera.ViewMatrix;
            float translationX = cameraMatrix[4, 1];
            float translationY = cameraMatrix[4, 2];
            float scaleX = cameraMatrix[1, 1];
            float scaleY = cameraMatrix[2, 2];
            //Convert the cursor screen pos into world position.
            cursorX += translationX;
            cursorY += -translationY;
            cursorX /= scaleX;
            cursorY /= -scaleY;
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
                bool colliding = cursorX >= minimumX && cursorX <= maximumX && cursorY >= minimumY && cursorY <= maximumY;
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
