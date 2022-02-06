using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Effects;
using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;
using static GJ2022.Managers.SignalHandler;

namespace GJ2022.Entities.Items.Clothing.Back
{
    public class Jetpack : Item, IEquippable
    {

        public Jetpack(Vector<float> position) : base(position)
        {
        }

        public override string Name => "Jetpack";

        public override string UiTexture => "jetpack";

        public InventorySlot Slots => InventorySlot.SLOT_BACK;

        public PawnHazards ProtectedHazards => PawnHazards.HAZARD_GRAVITY;

        public string EquipTexture => "jetpack";

        public override Renderable Renderable { get; set; } = new StandardRenderable("jetpack");

        public ClothingFlags ClothingFlags => ClothingFlags.NONE;

        public BodyCoverFlags CoverFlags => BodyCoverFlags.NONE;

        public void OnEquip(Pawn pawn, InventorySlot slot)
        {
            RegisterSignal(pawn, Signal.SIGNAL_ENTITY_MOVED, ParentMoveReact);
            Location = pawn;
        }

        public void OnUnequip(Pawn pawn, InventorySlot slot)
        {
            UnregisterSignal(pawn, Signal.SIGNAL_ENTITY_MOVED);
            Location = null;
            Position = pawn.Position;
        }

        private SignalResponse ParentMoveReact(object source, params object[] parameters)
        {
            Entity entity = source as Entity;
            new Sparkle((Vector<int>)entity.Position);
            return SignalResponse.NONE;
        }

    }
}
