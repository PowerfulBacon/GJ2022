using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns
{
    public class Dog : Pawn
    {

        public Dog(Vector<float> position) : base(position)
        {
        }

        protected override Renderable Renderable { get; set; } = new StandardRenderable("dog");

        protected override void AddEquipOverlay(InventorySlot targetSlot, IEquippable item)
        {
            return;
        }
    }
}
