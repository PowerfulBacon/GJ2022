using GJ2022.Entities.Items.Clothing;
using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Limbs.Felinid
{
    public class TailFelinid : Limb
    {
        public TailFelinid(Body body, BodySlots slot) : base(body, slot)
        {
        }

        public override BodySlots[] AllowedSlots => new BodySlots[] { BodySlots.SLOT_TAIL };

        public override LimbFlags DefaultLimbFlags => LimbFlags.NONE;

        public override float HighPressureDamage => 200;

        public override float LowPressureDamage => 20;

        public override float MaxHealth => 15;

        public override BodyCoverFlags CoverFlags => BodyCoverFlags.COVER_TAIL;

        public override void AddOverlay(Renderable renderable)
        {
            renderable.AddOverlay($"tail", new StandardRenderable($"mutant_bodyparts.m_tail_cat_FRONT"), Layers.LAYER_PAWN + 0.02f);
        }

        public override void RemoveOverlay(Renderable renderable)
        {
            renderable.RemoveOvelay($"tail");
        }

        public override void SetupOrgans(Pawn pawn, Body body)
        {
            return;
        }
    }
}
