using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs.BodyOrgans;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Human
{
    /// <summary>
    /// The body cannot be removed, so doesn't provide an overlay.
    /// Humans all have a base body for their main renderable.
    /// </summary>
    public class BodyHuman : Limb
    {

        public BodyHuman(Body body, BodySlots slot) : base(body, slot)
        {
            
        }

        public override BodySlots[] AllowedSlots => new BodySlots[] { BodySlots.SLOT_BODY };

        public override LimbFlags DefaultLimbFlags => LimbFlags.CRITICAL_LIMB | LimbFlags.NO_REMOVAL;

        public override float MaxHealth => 60;

        public override float HighPressureDamage => 200;

        public override float LowPressureDamage => 20;

        public virtual bool IsGendered { get; } = true;

        public override void SetupOrgans(Pawn pawn, Body body)
        {
            containedOrgans.Add(new Heart(pawn, body));
            containedOrgans.Add(new Liver(pawn, body));
            containedOrgans.Add(new Lung(pawn, body));
            containedOrgans.Add(new Stomach(pawn, body));
        }

        public override void AddOverlay(Renderable renderable)
        {
            string gender_extension = IsGendered ? $"_{Body.GetGenderText()}" : "";
            renderable.AddOverlay($"body", new StandardRenderable($"human_body{gender_extension}"), Layers.LAYER_PAWN + 0.01f);
        }

        public override void RemoveOverlay(Renderable renderable)
        {
            renderable.RemoveOvelay($"body");
        }

        public override void UpdateDamageOverlays(Renderable renderable)
        {
            if (renderable.HasOverlay($"dambody"))
                renderable.RemoveOvelay($"dambody");
            if (Health <= 0)
                renderable.AddOverlay($"dambody", new StandardRenderable($"brute_body_2"), Layers.LAYER_PAWN + 0.03f);
            else if (Health < MaxHealth)
                renderable.AddOverlay("dambody", new StandardRenderable($"brute_body_0"), Layers.LAYER_PAWN + 0.03f);
        }

    }
}
