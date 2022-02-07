using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Atmospherics;
using GJ2022.Atmospherics.Gasses;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items.Clothing;
using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.Tank
{
    public class OxygenTank : GasTank, IEquippable
    {

        public OxygenTank(Vector<float> position) : base(position)
        {
            //12 moles = 10 minutes of oxygen
            //40 litre tank
            //295.15k temperature
            //~740kPa pressure
            ContainedAtmosphere = new Atmosphere(AtmosphericConstants.IDEAL_TEMPERATURE, new PressurisedGas(Oxygen.Singleton, 12));
            ContainedAtmosphere.SetVolume(40);
        }

        public override Atmosphere ContainedAtmosphere { get; }

        public override string Name => "Oxygen Tank";

        public override string UiTexture => "tank.oxygen";

        public override Renderable Renderable { get; set; } = new StandardRenderable("tank.oxygen");

        public bool AppendSlotToIconState => false;

        public InventorySlot Slots => InventorySlot.SLOT_BACK;

        public PawnHazards ProtectedHazards => PawnHazards.NONE;

        public string EquipTexture => "back.oxygen";

        public ClothingFlags ClothingFlags => ClothingFlags.NONE;

        public BodyCoverFlags CoverFlags => BodyCoverFlags.NONE;

        public void OnEquip(Pawn pawn, InventorySlot slot)
        {
        }

        public void OnUnequip(Pawn pawn, InventorySlot slot)
        {
        }
    }
}
