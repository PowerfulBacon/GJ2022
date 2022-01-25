using GJ2022.Entities.Blueprints;
using GJ2022.Game.Construction.Blueprints;
using GJ2022.Game.Construction.BlueprintSets;
using GJ2022.GlobalDataComponents;
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
        private bool cancelDisable = false;

        private Vector<float> dragStartPoint;
        private Vector<float> dragEndPoint;

        private static Dictionary<Vector<float>, Blueprint> dragHighlights = new Dictionary<Vector<float>, Blueprint>();

        private BlueprintSet selectedBlueprint = null;

        private Blueprint buildModeHighlight;

        public override void Fire(Window window)
        {
            //Don't fire if build mode isn't active
            if (!buildModeActive)
                return;
            //Exit build mode
            if (Glfw.GetKey(window, Keys.Escape) == InputState.Press)
            {
                DisableBuildMode();
                return;
            }
            //Ignore if we aren't the primary hook
            if (!MouseHookTracker.HasMouseControl("buildmode") && !isDragging)
                return;
            //Check if dragging
            if (!isDragging)
            {
                //Mouse button isn't down.
                if (Glfw.GetMouseButton(window, MouseButton.Left) != InputState.Press)
                {
                    if (buildModeHighlight == null)
                        buildModeHighlight = new Blueprint((Vector<int>)ScreenToWorldHelper.GetWorldCoordinates(window), selectedBlueprint.BlueprintDetail);
                    else
                        buildModeHighlight.Position = ScreenToWorldHelper.GetWorldTile(window);
                    cancelDisable = false;
                    //If we press the right mouse button, disable build mode
                    if (Glfw.GetMouseButton(window, MouseButton.Right) == InputState.Press)
                    {
                        DisableBuildMode();
                        return;
                    }
                    return;
                }
                else if (cancelDisable)
                    return;
                //Remove the build mode highlight
                buildModeHighlight?.Destroy();
                buildModeHighlight = null;
                //Mouse button is down, start dragging
                isDragging = true;
                //Mark the location of where we started dragging from
                dragStartPoint = ScreenToWorldHelper.GetWorldTile(window);
            }
            //Draw highlights
            dragEndPoint = ScreenToWorldHelper.GetWorldTile(window);

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
                        blueprintDetail.BlueprintType,
                        position,
                        blueprintDetail) as Blueprint;
                    dragHighlights.Add(position, blueprint);
                }
            }

            //Check if right click (cancel)
            if (Glfw.GetMouseButton(window, MouseButton.Right) == InputState.Press)
            {
                ClearDragHighlights();
                isDragging = false;
                cancelDisable = true;
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

        public void ActivateBuildMode(BlueprintSet buildModeBlueprint)
        {
            selectedBlueprint = buildModeBlueprint;
            if (buildModeActive)
                return;
            buildModeActive = true;
            MouseHookTracker.AddHook("buildmode", 50);
        }

        public void DisableBuildMode()
        {
            if (!buildModeActive)
                return;
            buildModeActive = false;
            buildModeHighlight?.Destroy();
            buildModeHighlight = null;
            ClearDragHighlights();
            MouseHookTracker.RemoveHook("buildmode");
        }

    }
}
