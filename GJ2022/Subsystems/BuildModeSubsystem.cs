using GJ2022.Entities.Abstract;
using GJ2022.Entities.Blueprints;
using GJ2022.Game.Construction;
using GJ2022.Game.Construction.Blueprints;
using GJ2022.Game.Construction.BlueprintSets;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;
using GLFW;
using System;
using System.Collections.Generic;

namespace GJ2022.Subsystems
{
    class BuildModeSubsystem : Subsystem
    {

        public static BuildModeSubsystem Singleton { get; } = new BuildModeSubsystem();

        public override int sleepDelay => 50;

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_PROCESSING;

        private bool buildModeActive = false;
        private bool isDragging = false;

        private Vector<float> dragStartPoint;
        private Vector<float> dragEndPoint;

        private static Dictionary<Vector<float>, Blueprint> dragHighlights = new Dictionary<Vector<float>, Blueprint>();

        private BlueprintSet selectedBlueprint = new BuildingBlueprint();

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

            //Create drag highlights
            ClearDragHighlights();
            for (int x = (int)Math.Min(dragStartPoint[0], dragEndPoint[0]); x <= (int)Math.Max(dragStartPoint[0], dragEndPoint[0]); x++)
            {
                for (int y = (int)Math.Min(dragStartPoint[1], dragEndPoint[1]); y <= (int)Math.Max(dragStartPoint[1], dragEndPoint[1]); y++)
                {
                    //Check for border
                    bool isBorder = x == (int)dragStartPoint[0] || x == (int)dragEndPoint[0] || y == (int)dragStartPoint[1] || y == (int)dragEndPoint[1];
                    //Blueprint
                    Vector<float> position = new Vector<float>(x, y, 2);
                    BlueprintDetail blueprintDetail = isBorder ? selectedBlueprint.BlueprintDetail : selectedBlueprint.FillerBlueprint;
                    Blueprint blueprint = Activator.CreateInstance(
                        position,
                        blueprintDetail) as Blueprint;
                    BlueprintRenderSystem.Singleton.StartRendering(blueprint);
                    dragHighlights.Add(position, blueprint);
                }
            }

            //Check if right click (cancel)
            if (Glfw.GetMouseButton(window, MouseButton.Right) == InputState.Press)
            {
                ClearDragHighlights();
                isDragging = false;
            }
            //Check if released left click (confirm)
            else if (Glfw.GetMouseButton(window, MouseButton.Left) == InputState.Release)
            {
                ConfirmBuild();
                isDragging = false;
            }
        }

        public override void InitSystem()
        { }

        protected override void AfterWorldInit()
        { }

        private void ConfirmBuild()
        {
            //Copy the drag highlight list into the AI controller system
            foreach (Vector<float> position in dragHighlights.Keys)
            {
                PawnControllerSystem.QueueBlueprint(position, dragHighlights[position], selectedBlueprint.BlueprintDetail.BlueprintLayer);
            }
            //Reset our list
            dragHighlights.Clear();
        }

        private void ClearDragHighlights()
        {
            foreach (Blueprint bp in dragHighlights.Values)
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
