using GJ2022.Atmospherics;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items.Clothing.Head.HardsuitHelmet;
using GJ2022.Entities.Pawns;
using GJ2022.Game.GameWorld;
using GJ2022.PawnBehaviours;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;
using static GJ2022.Managers.SignalHandler;

namespace GJ2022.Entities.Items.Clothing.Body
{
    public class SyndicateHardsuit : Item, IEquippable
    {

        public SyndicateHardsuit(Vector<float> position) : base(position)
        {
            storedHelmet = new HardsuitHelmet(position);
            storedHelmet.Location = this;
        }

        public override string Name => "Syndicate Hardsuit";

        public override string UiTexture => "syndicate_hardsuit";

        public InventorySlot Slots => InventorySlot.SLOT_BODY;

        public PawnHazards ProtectedHazards => PawnHazards.HAZARD_LOW_PRESSURE;

        public string EquipTexture => "syndicate_hardsuit";

        public override Renderable Renderable { get; set; } = new StandardRenderable("syndicate_hardsuit");

        public ClothingFlags ClothingFlags => ClothingFlags.HIDE_TAIL;

        public BodyCoverFlags CoverFlags => BodyCoverFlags.COVER_FEET | BodyCoverFlags.COVER_ARMS | BodyCoverFlags.COVER_BODY | BodyCoverFlags.COVER_FEET | BodyCoverFlags.COVER_HANDS | BodyCoverFlags.COVER_LEGS | BodyCoverFlags.COVER_TAIL;

        public bool AppendSlotToIconState => true;

        private HardsuitHelmet storedHelmet;

        public void OnEquip(Pawn pawn, InventorySlot slot)
        {
            Location = pawn;
            RegisterSignal(pawn, Signal.SIGNAL_ENTITY_MOVED, ParentMoveReact);
        }

        public void OnUnequip(Pawn pawn, InventorySlot slot)
        {
            Location = null;
            Position = pawn.Position;
            UnregisterSignal(pawn, Signal.SIGNAL_ENTITY_MOVED);
        }

        private SignalResponse ParentMoveReact(object source, params object[] parameters)
        {
            Pawn pawn = source as Pawn;
            //Get current position
            Atmosphere atmosphere = World.GetTurf((int)pawn.Position[0], (int)pawn.Position[1])?.Atmosphere?.ContainedAtmosphere;
            //Check for pressure hazard
            bool shouldHelmet = (atmosphere == null || (pawn.PawnBody.InsertedLimbs.ContainsKey(Pawns.Health.BodySlots.SLOT_HEAD) && (atmosphere.KiloPascalPressure < pawn.PawnBody.InsertedLimbs[Pawns.Health.BodySlots.SLOT_HEAD].LowPressureDamage + 20 || atmosphere.KiloPascalPressure > pawn.PawnBody.InsertedLimbs[Pawns.Health.BodySlots.SLOT_HEAD].HighPressureDamage - 20)));
            //Put up the helmet
            if(shouldHelmet)
                DeployHelmet(pawn);
            //Take down the helmet
            else
                UndeployHelmet(pawn);
            return SignalResponse.NONE;
        }

        public void DeployHelmet(Pawn pawn)
        {
            pawn.TryEquipItem(InventorySlot.SLOT_HEAD, storedHelmet);
        }

        public void UndeployHelmet(Pawn pawn)
        {

        }

    }
}
