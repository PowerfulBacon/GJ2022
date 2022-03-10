using GJ2022.Components;
using GJ2022.Entities.Areas;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.ComponentInterfaces.MouseEvents;
using GJ2022.Entities.Pawns;
using GJ2022.Game.GameWorld;
using GJ2022.Managers.Stockpile;
using GJ2022.PawnBehaviours.PawnActions;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;
using GLFW;
using System;

namespace GJ2022.Entities.Items
{
    public class Item : Entity, IDestroyable, IMoveBehaviour, IMouseRightPress
    {

        public bool Destroyed { get; private set; } = false;

        public virtual string UiTexture { get; private set; }

        public CursorSpace PositionSpace => CursorSpace.WORLD_SPACE;

        public float WorldX => Position[0] - 0.5f;

        public float WorldY => Position[1] - 0.5f;

        public float Width => 1.0f;

        public float Height => 1.0f;

        public Item() : base()
        {
            MouseCollisionSubsystem.Singleton.StartTracking(this);
        }

        public Item(Vector<float> position) : base(position, Layers.LAYER_ITEM)
        {
            MouseCollisionSubsystem.Singleton.StartTracking(this);
        }

        public override bool Destroy()
        {
            base.Destroy();
            Destroyed = true;
            World.RemoveItem((int)Position[0], (int)Position[1], this);
            World.GetArea((int)Position[0], (int)Position[1])?.SendSignal(Signal.SIGNAL_AREA_CONTENTS_REMOVED, this);
            //Handle inventory removal
            if (Location is Pawn holder)
            {
                //This causes the pawn to update its inventory
                holder.GetHeldItems();
            }
            return true;
        }

        /// <summary>
        /// On move behaviour.
        /// Update ourself in the world list
        /// </summary>
        public void OnMoved(Vector<float> oldPosition)
        {
            if ((int)oldPosition[0] == (int)Position[0] && (int)oldPosition[1] == (int)Position[1])
                return;
            World.RemoveItem((int)oldPosition[0], (int)oldPosition[1], this);
            World.AddItem((int)Position[0], (int)Position[1], this);
            //Calculate stockpile
            World.GetArea((int)oldPosition[0], (int)oldPosition[1])?.SendSignal(Signal.SIGNAL_AREA_CONTENTS_REMOVED, this);
            World.GetArea((int)Position[0], (int)Position[1])?.SendSignal(Signal.SIGNAL_AREA_CONTENTS_ADDED, this);
        }

        public void OnMoved(Entity oldLocation)
        {
            if (oldLocation == Location)
                return;
            if (oldLocation != null)
            {
                MouseCollisionSubsystem.Singleton.StartTracking(this);
                return;
            }
            MouseCollisionSubsystem.Singleton.StopTracking(this);
            World.RemoveItem((int)Position[0], (int)Position[1], this);
            World.GetArea((int)Position[0], (int)Position[1])?.SendSignal(Signal.SIGNAL_AREA_CONTENTS_REMOVED, this);
        }

        public void UpdateCount()
        {
            StockpileManager.CountItems(GetType());
        }

        public virtual int Count()
        {
            return Convert.ToInt32(SendSignalSynchronously(Signal.SIGNAL_GET_COUNT) ?? 1);
        }

        public void OnRightPressed(Window window)
        {
            if (Location != null)
                return;
            if (PawnControllerSystem.Singleton.SelectedPawn == null)
                return;
            if (SendSignalSynchronously(Signal.SIGNAL_RIGHT_CLICKED)?.Equals(true) ?? false)
                return;
            PawnControllerSystem.Singleton.SelectedPawn.behaviourController?.PawnActionIntercept(new HaulItems(this));
            /*UserInterfaceButton button = new UserInterfaceButton(
                WorldToScreenHelper.GetScreenCoordinates(window, Position) + CoordinateHelper.PixelsToScreen(0, 80),
                CoordinateHelper.PixelsToScreen(300, 80),
                "Pickup",
                CoordinateHelper.PixelsToScreen(80),
                Colour.UserInterfaceColour);*/
        }

        public override void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "UiTexture":
                    UiTexture = (string)property;
                    return;
            }
            base.SetProperty(name, property);
        }
    }
}
