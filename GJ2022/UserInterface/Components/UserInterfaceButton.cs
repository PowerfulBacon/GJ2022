﻿using GJ2022.Entities.ComponentInterfaces.MouseEvents;
using GJ2022.Managers;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Rendering.Text;
using GJ2022.Subsystems;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.UserInterface.Components
{
    public class UserInterfaceButton : UserInterfaceComponent, IMouseEnter, IMouseExit, IMousePress
    {

        public delegate void OnButtonPressed();
        public OnButtonPressed onButtonPressed;

        private static int ButtonCount = 0;
        private int uniqueId = ButtonCount++;

        public override Renderable Renderable { get; }

        public TextObject TextObject { get; }

        public CursorSpace PositionSpace { get; } = CursorSpace.SCREEN_SPACE;

        public float WorldX => _position.X * (1080.0f / 1920.0f) - Width / 2;

        public float WorldY => -_position.Y - Height / 2;

        public float Width => Scale[0] * (1080.0f / 1920.0f);

        public float Height => Scale[1];

        public override Vector<float> Scale { get; }

        private Vector<float> _position;
        public override Vector<float> Position
        {
            get { return _position; }
            set
            {
                _position = value;
                Renderable?.UpdatePosition(value);
                TextObject.Position = value - CoordinateHelper.PixelsToScreen(new Vector<float>(90, 20));
            }
        }

        public UserInterfaceButton(Vector<float> position, Vector<float> scale, string text, float textScale, Colour colour, CursorSpace cursorSpace = CursorSpace.SCREEN_SPACE)
        {
            PositionSpace = cursorSpace;
            PositionMode = cursorSpace == CursorSpace.SCREEN_SPACE ? PositionModes.SCREEN_POSITION : PositionModes.WORLD_POSITION;
            Scale = scale;
            Renderable = new ButtonRenderable(position, PositionMode, colour, scale);
            TextObject = new TextObject(text, Colour.White, position - CoordinateHelper.PixelsToScreen(new Vector<float>(90, 20)), cursorSpace == CursorSpace.SCREEN_SPACE ? TextObject.PositionModes.SCREEN_POSITION : TextObject.PositionModes.WORLD_POSITION, textScale);
            _position = position;
            //Track
            MouseCollisionSubsystem.Singleton.StartTracking(this);
        }

        public void OnMouseExit()
        {
            Renderable.buttonStateChangeHandler?.Invoke(false);
            MouseHookTracker.RemoveHook($"button{uniqueId}");
        }

        public void OnMouseEnter()
        {
            Renderable.buttonStateChangeHandler?.Invoke(true);
            MouseHookTracker.AddHook($"button{uniqueId}", 400);
        }

        public void OnPressed()
        {
            onButtonPressed?.Invoke();
        }

        public override void Hide()
        {
            Renderable.StopRendering();
            TextObject.StopRendering();
            MouseCollisionSubsystem.Singleton.StopTracking(this);
        }

        public override void Show()
        {
            Renderable.StartRendering();
            TextObject.StartRendering();
            MouseCollisionSubsystem.Singleton.StartTracking(this);
        }
    }
}
