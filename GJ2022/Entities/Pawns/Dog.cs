using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Bodies.Instances;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Pawns
{
    public class Dog : Pawn
    {

        public Dog(Vector<float> position) : base(position)
        {
        }

        public override Body PawnBody => new BodyDog();

        protected override Renderable Renderable { get; set; } = new StandardRenderable("dog");

        protected override void AddEquipOverlay(InventorySlot targetSlot, IEquippable item)
        {
            return;
        }
    }
}
