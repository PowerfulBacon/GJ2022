using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.Clothing.Body
{
    class SpaceSuit : Item, IEquippable
    {

        public SpaceSuit(Vector<float> position) : base(position)
        {
        }

        public override string Name => "Space Suit";

        public override string UiTexture => "spacesuit";

        public InventorySlot Slots => InventorySlot.SLOT_BODY;

        public PawnHazards ProtectedHazards => PawnHazards.HAZARD_LOW_PRESSURE;

        public string equipTexture => "spacesuit";

        protected override Renderable Renderable { get; set; } = new StandardRenderable("spacesuit");

        public void OnEquip(Pawn pawn, InventorySlot slot)
        {
            Location = pawn;
        }

        public void OnUnequip(Pawn pawn, InventorySlot slot)
        {
            Location = null;
            Position = pawn.Position;
        }

    }
}
