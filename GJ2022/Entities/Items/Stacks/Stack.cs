using GJ2022.Rendering.Text;
using GJ2022.Utility.MathConstructs;
using System;

namespace GJ2022.Entities.Items.Stacks
{
    public abstract class Stack : Item
    {

        protected Stack(Vector<float> position, int maxStackSize = 50, int stackSize = 1) : base(position)
        {
            MaxStackSize = maxStackSize;
            StackSize = stackSize;
            textObjectOffset = new Vector<float>(0, -0.6f);
            attachedTextObject = new TextObject($"{StackSize}", Colour.White, Position + textObjectOffset, TextObject.PositionModes.WORLD_POSITION, 0.4f);
        }

        public int MaxStackSize { get; }

        public int StackSize { get; set; }

        public override int Count()
        {
            return StackSize;
        }

        public Stack Take(int amount)
        {
            if (amount >= StackSize)
            {
                return this;
            }
            else
            {
                StackSize -= amount;
                UpdateStackSize();
                return (Stack)Activator.CreateInstance(GetType(), Position, MaxStackSize, amount);
            }
        }

        private void UpdateStackSize()
        {
            attachedTextObject.Text = $"{StackSize}";
        }

    }
}
