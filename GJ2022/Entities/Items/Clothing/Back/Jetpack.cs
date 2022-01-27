using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        protected override Renderable Renderable { get; set; } = new StandardRenderable("jetpack");

    }
}
