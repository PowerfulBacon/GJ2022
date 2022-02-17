using GJ2022.Atmospherics;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.Clothing.Mask
{
    public class BreathMask : Item, IBreathMask, IEquippable
    {
        public BreathMask(Vector<float> position) : base(position)
        {
        }

        public bool AppendSlotToIconState => false;

        public InventorySlot Slots => InventorySlot.SLOT_MASK;

        public PawnHazards ProtectedHazards => PawnHazards.NONE;

        public string EquipTexture => "mask.breath";

        public ClothingFlags ClothingFlags => ClothingFlags.NONE;

        public BodyCoverFlags CoverFlags => BodyCoverFlags.COVER_MOUTH;

        public override string Name => "Breath Mask";

        public override string UiTexture => "masks.breath";

        public override Renderable Renderable { get; set; } = new StandardRenderable("masks.breath");

        public Atmosphere GetBreathSource()
        {
            Pawn pawn = Location as Pawn;
            if (pawn == null)
                return null;
            //Locate a valid tank
            foreach (IEquippable equippable in pawn.EquippedItems.Values)
            {
                //TODO: Convert tank to an interface
                //GasTank tank = equippable as GasTank;
                //if (tank != null)
                //{
                //    return tank.ContainedAtmosphere;
                //}
            }
            //No valid tanks found :(
            return null;
        }

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
