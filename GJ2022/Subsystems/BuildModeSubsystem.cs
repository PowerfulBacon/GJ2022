using GJ2022.Entities.Abstract;
using GJ2022.GlobalDataComponents;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;
using GLFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Subsystems
{
    class BuildModeSubsystem : Subsystem
    {

        public static BuildModeSubsystem Singleton { get; } = new BuildModeSubsystem();

        public override int sleepDelay => 50;

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_PROCESSING;

        private bool buildModeActive = false;
        private bool isDragging = false;

        private Vector dragStartPoint;
        private Vector dragEndPoint;

        private List<Blueprint> dragHighlights = new List<Blueprint>();

        public override void Fire(Window window)
        {
            //Don't fire if build mode isn't active
            if (!buildModeActive)
                return;
            //Check if dragging
            if (!isDragging)
            {
                //Mouse button isn't down.
                if (Glfw.GetMouseButton(window, MouseButton.Left) != InputState.Press)
                    return;
                //Mouse button is down, start dragging
                isDragging = true;
                //Mark the location of where we started dragging from
                dragStartPoint = ScreenToWorldHelper.GetWorldCoordinates(window);
            }
            //Draw highlights
            dragEndPoint = ScreenToWorldHelper.GetWorldCoordinates(window);
            //Check if right click (cancel)
            if (Glfw.GetMouseButton(window, MouseButton.Right) == InputState.Press)
            {
                ClearDragHighlights();
            }
            //Check if released left click (confirm)
            else if (Glfw.GetMouseButton(window, MouseButton.Left) == InputState.Release)
            {
                ConfirmBuild();
            }
        }

        public override void InitSystem()
        { }

        protected override void AfterWorldInit()
        { }

        private void ConfirmBuild()
        {

        }

        private void ClearDragHighlights()
        {
            foreach(Blueprint bp in dragHighlights)
            {
                bp.Destroy();
            }
            dragHighlights.Clear();
        }

        public void ActivateBuildMode()
        {
            buildModeActive = true;
        }

        public void DisableBuildMode()
        {
            buildModeActive = false;
        }

    }
}
