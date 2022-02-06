using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Items.Clothing.Head.HardsuitHelmet
{
    public class HardsuitHelmet : Item, IEquippable
    {
        public HardsuitHelmet(Vector<float> position) : base(position)
        {
        }

        public InventorySlot Slots => InventorySlot.SLOT_HEAD;

        public PawnHazards ProtectedHazards => PawnHazards.HAZARD_LOW_PRESSURE | PawnHazards.HAZARD_HIGH_PRESSURE;

        public string EquipTexture => "head.hardsuit1-syndi";

        public ClothingFlags ClothingFlags => ClothingFlags.HIDE_EYES | ClothingFlags.HIDE_HAIR;

        public BodyCoverFlags CoverFlags => BodyCoverFlags.COVER_HEAD | BodyCoverFlags.COVER_MOUTH;

        public override string Name => "Syndicate Hardsuit Helmet";

        public override string UiTexture => "hats.hardsuit1-syndi";

        public override Renderable Renderable { get; set; } = new StandardRenderable("hats.hardsuit1-syndi");

        public bool AppendSlotToIconState => false;

        public void OnEquip(Pawn pawn, InventorySlot slot)
        {
        }

        public void OnUnequip(Pawn pawn, InventorySlot slot)
        {
        }
    }
}
