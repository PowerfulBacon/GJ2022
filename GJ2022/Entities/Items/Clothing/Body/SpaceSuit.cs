using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.Clothing.Body
{
    public class SpaceSuit : Item, IEquippable
    {

        public SpaceSuit(Vector<float> position) : base(position)
        {
        }

        public override string Name => "Space Suit";

        public override string UiTexture => "spacesuit";

        public InventorySlot Slots => InventorySlot.SLOT_BODY;

        public PawnHazards ProtectedHazards => PawnHazards.HAZARD_LOW_PRESSURE;

        public string EquipTexture => "spacesuit";

        public override Renderable Renderable { get; set; } = new StandardRenderable("spacesuit");

        public ClothingFlags ClothingFlags => ClothingFlags.HIDE_TAIL;

        public BodyCoverFlags CoverFlags => BodyCoverFlags.COVER_FEET | BodyCoverFlags.COVER_ARMS | BodyCoverFlags.COVER_BODY | BodyCoverFlags.COVER_FEET | BodyCoverFlags.COVER_HANDS | BodyCoverFlags.COVER_LEGS | BodyCoverFlags.COVER_TAIL;

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
