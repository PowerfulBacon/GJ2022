using GJ2022.Entities.Items.Clothing;
using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Limbs
{
    public class ArmHuman : Limb
    {
        public ArmHuman(Body body, BodySlots slot) : base(body, slot)
        {
        }

        public override BodySlots[] AllowedSlots => new BodySlots[] { BodySlots.SLOT_ARM_LEFT, BodySlots.SLOT_ARM_RIGHT };

        public override LimbFlags DefaultLimbFlags => LimbFlags.NONE;

        public override float MaxHealth => 35;

        public override float ManipulationFactor => 50;

        public override float HighPressureDamage => 200;

        public override float LowPressureDamage => 20;

        public override BodyCoverFlags CoverFlags => BodyCoverFlags.COVER_ARMS | BodyCoverFlags.COVER_HANDS;

        public override void SetupOrgans(Pawn pawn, Body body)
        {
            //TODO
            return;
        }

        public override void AddOverlay(Renderable renderable)
        {
            string direction = InsertedSlot == BodySlots.SLOT_ARM_LEFT ? "left" : "right";
            renderable.AddOverlay($"{direction}arm", new StandardRenderable($"human_{direction}arm"), Layers.LAYER_PAWN + 0.01f);
            renderable.AddOverlay($"{direction}hand", new StandardRenderable($"human_{direction}hand"), Layers.LAYER_PAWN + 0.02f);
        }

        public override void RemoveOverlay(Renderable renderable)
        {
            string direction = InsertedSlot == BodySlots.SLOT_ARM_LEFT ? "left" : "right";
            renderable.RemoveOvelay($"{direction}arm");
            renderable.RemoveOvelay($"{direction}hand");
        }

        public override void UpdateDamageOverlays(Renderable renderable)
        {
            string direction = InsertedSlot == BodySlots.SLOT_ARM_LEFT ? "left" : "right";
            if (renderable.HasOverlay($"{direction}damarm"))
                renderable.RemoveOvelay($"{direction}damarm");
            if (Health <= 0)
                renderable.AddOverlay($"{direction}damarm", new StandardRenderable($"brute_{direction}arm_2"), Layers.LAYER_PAWN + 0.03f);
            else if (Health < MaxHealth)
                renderable.AddOverlay($"{direction}damarm", new StandardRenderable($"brute_{direction}arm_0"), Layers.LAYER_PAWN + 0.03f);
        }
    }
}
