using GJ2022.Entities.ComponentInterfaces.MouseEvents;
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

        public static MouseCollisionSubsystem Singleton { get; } = new MouseCollisionSubsystem();

        //50 FPS
        public override int sleepDelay => 20;

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_PROCESSING;

        //Tracking mouse enter events
        private HashSet<IMouseEvent> trackingEvents = new HashSet<IMouseEvent>();

        public void StartTracking(IMouseEvent tracker)
        {
            trackingEvents.Add(tracker);
        }

        public void StopTracking(IMouseEvent tracker)
        {
            trackingEvents.Remove(tracker);
        }

        public override void Fire(Window window)
        {
            //Calculate proportion position of the mouse
            double cursorX;
            double cursorY;
            Glfw.GetCursorPosition(window, out cursorX, out cursorY);
            //Get the size of the window
            int windowWidth;
            int windowHeight;
            Glfw.GetWindowSize(window, out windowWidth, out windowHeight);
            //Go through all tracking events and handle them
            for (int i = trackingEvents.Count - 1; i >= 0; i--)
            {
                IMouseEvent mouseEventHolder = trackingEvents[i];
            }
        }

        public override void InitSystem()
        { }

        protected override void AfterWorldInit()
        { }

    }
}
