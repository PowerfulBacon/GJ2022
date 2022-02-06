using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Bodies.Instances;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Pawns.Mobs.Humans
{
    public class Human : Pawn
    {

        public Human(Vector<float> position) : base(position)
        {
            Renderable.UpdateDirection(Directions.EAST);
        }

        public override bool UsesLimbOverlays => true;

        public override Body PawnBody { get; } = new BodyHuman();

        public override Renderable Renderable { get; set; } = new StandardRenderable("transparent");

        protected override void AddEquipOverlay(InventorySlot targetSlot, IEquippable item)
        {
            Renderable.AddOverlay($"wear_{InventoryHelper.GetSlotAppend(targetSlot)}", new StandardRenderable($"{item.EquipTexture}_{InventoryHelper.GetSlotAppend(targetSlot)}"), Layers.LAYER_PAWN + 0.08f);
        }
    }
}
