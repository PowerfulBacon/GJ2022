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

        public int MaxStackSize { get; }

        public int StackSize { get; set; }

        public override void OnComponentAdd()
        {
            Parent.RegisterSignal(Signal.SIGNAL_GET_COUNT, 5, ReturnStackSize);
            //Setup the text display
            Entity parent = Parent as Entity;
            parent.textObjectOffset = new Vector<float>(0, -0.6f);
            parent.attachedTextObject = new TextObject($"{StackSize}", Colour.White, parent.Position + parent.textObjectOffset, TextObject.PositionModes.WORLD_POSITION, 0.4f);
        }

        public override void OnComponentRemove()
        {
            Parent.UnregisterSignal(Signal.SIGNAL_GET_COUNT, ReturnStackSize);
            //Remove the text
            Entity parent = Parent as Entity;
            parent.attachedTextObject.StopRendering();
            parent.attachedTextObject = null;
        }

        private object ReturnStackSize(object source, params object[] parameters) => StackSize;

        public override void SetProperty(string name, object property)
        {
            throw new NotImplementedException();
        }
    }
}
