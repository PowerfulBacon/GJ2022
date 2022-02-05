using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs.HeadOrgans;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Human
{
    public class HeadHuman : Limb
    {
        public HeadHuman(Body body, BodySlots slot) : base(body, slot)
        {
        }

        public override BodySlots[] AllowedSlots => new BodySlots[] { BodySlots.SLOT_HEAD };

        public override LimbFlags DefaultLimbFlags => LimbFlags.CRITICAL_LIMB | LimbFlags.NO_INSERTION;

        public override float MaxHealth => 45;

        public override void SetupOrgans(Pawn pawn, Body body)
        {
            //TODO
            containedOrgans.Add(new Brain(pawn, body));
        }

        public override void AddOverlay(Renderable renderable)
        {
            //TODO: Genders
            renderable.AddOverlay($"head", new StandardRenderable($"human_head_male"), Layers.LAYER_PAWN + 0.01f);
        }

        public override void RemoveOverlay(Renderable renderable)
        {
            renderable.RemoveOvelay($"head");
        }
    }
}
