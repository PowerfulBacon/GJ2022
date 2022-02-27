using GJ2022.Components.Generic;
using GJ2022.Entities;
using GJ2022.Rendering.Text;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Items
{
    public class Component_Stackable : Component
    {

        public int MaxStackSize { get; private set; } = 1;

        public int StackSize { get; private set; } = 1;

        public string Key { get; private set; }

        private Component_Tracked stackableTrackComponent;

        public override void OnComponentAdd()
        {
            //Add a component to the parent for tracking stackable items (We need to track for merges)
            stackableTrackComponent = new Component_Tracked();
            stackableTrackComponent.Key = $"stackable_{Key}";
            Parent.AddComponent(stackableTrackComponent);
            //Register signals
            Parent.RegisterSignal(Signal.SIGNAL_GET_COUNT, 5, ReturnStackSize);
            Parent.RegisterSignal(Signal.SIGNAL_SET_STACK_SIZE, -1, SetStackSize);
            //Setup the text display
            Entity parent = Parent as Entity;
            parent.textObjectOffset = new Vector<float>(0, -0.6f);
            parent.attachedTextObject = new TextObject($"{StackSize}", Colour.White, parent.Position + parent.textObjectOffset, TextObject.PositionModes.WORLD_POSITION, 0.4f);
        }

        public override void OnComponentRemove()
        {
            //Remove the tracking component
            Parent.RemoveComponent(stackableTrackComponent);
            stackableTrackComponent = null;
            //Unregister signals
            Parent.UnregisterSignal(Signal.SIGNAL_GET_COUNT, ReturnStackSize);
            Parent.UnregisterSignal(Signal.SIGNAL_SET_STACK_SIZE, SetStackSize);
            //Remove the text
            Entity parent = Parent as Entity;
            parent.attachedTextObject.StopRendering();
            parent.attachedTextObject = null;
        }

        private object SetStackSize(object source, params object[] parameters)
        {
            StackSize = Math.Min(Convert.ToInt32(parameters[0]), MaxStackSize);
            Entity parent = Parent as Entity;
            parent.attachedTextObject.Text = $"{StackSize}";
            return null;
        }

        private object ReturnStackSize(object source, params object[] parameters) => StackSize;

        public override void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "StackSize":
                    StackSize = Convert.ToInt32(property);
                    return;
                case "MaxStackSize":
                    MaxStackSize = Convert.ToInt32(property);
                    return;
                case "Key":
                    Key = (string)property;
                    return;
            }
        }
    }
}
