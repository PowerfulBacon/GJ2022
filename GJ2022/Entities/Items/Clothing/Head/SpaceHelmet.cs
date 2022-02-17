using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.Clothing.Head
{
    public class SpaceHelmet : Item, IEquippable
    {
        public SpaceHelmet(Vector<float> position) : base(position)
        {
        }

        public InventorySlot Slots => InventorySlot.SLOT_HEAD;

        public PawnHazards ProtectedHazards => PawnHazards.HAZARD_LOW_PRESSURE;

        public string EquipTexture => "head.space";

        public ClothingFlags ClothingFlags => ClothingFlags.HIDE_EYES | ClothingFlags.HIDE_HAIR;

        public BodyCoverFlags CoverFlags => BodyCoverFlags.COVER_HEAD | BodyCoverFlags.COVER_MOUTH;

        public override string Name => "Space Helmet";

        public override string UiTexture => "hats.space";

        public override Renderable Renderable { get; set; } = new StandardRenderable("hats.space");

        public bool AppendSlotToIconState => false;

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
