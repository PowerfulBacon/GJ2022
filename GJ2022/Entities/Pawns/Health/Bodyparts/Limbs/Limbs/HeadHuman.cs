using GJ2022.Entities.Items.Clothing;
using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Entities.Pawns.Health.Bodyparts.Organs.HeadOrgans;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Limbs
{
    public class HeadHuman : Limb
    {
        public HeadHuman(Body body, BodySlots slot) : base(body, slot)
        {
        }

        public override BodySlots[] AllowedSlots => new BodySlots[] { BodySlots.SLOT_HEAD };

        public override LimbFlags DefaultLimbFlags => LimbFlags.CRITICAL_LIMB | LimbFlags.NO_INSERTION;

        public override float MaxHealth => 45;

        public override float HighPressureDamage => 200;

        public override float LowPressureDamage => 20;

        public virtual bool IsGendered { get; } = true;

        public virtual bool HasHair { get; } = true;

        public override BodyCoverFlags CoverFlags => BodyCoverFlags.COVER_HEAD;

        private string haircut;

        public override void SetupOrgans(Pawn pawn, Body body)
        {
            containedOrgans.Add(new Brain(pawn, body));
            containedOrgans.Add(new Eye(pawn, body));
            containedOrgans.Add(new Eye(pawn, body));
            containedOrgans.Add(new Nose(pawn, body));
            containedOrgans.Add(new Tongue(pawn, body));
            containedOrgans.Add(new Ear(pawn, body));
            containedOrgans.Add(new Ear(pawn, body));
            haircut = body.GetRandomHaircut();
        }

        public override void AddOverlay(Renderable renderable)
        {
            string gender_extension = IsGendered ? $"_{Body.GetGenderText()}" : "";
            renderable.AddOverlay($"head", new StandardRenderable($"human_head{gender_extension}"), Layers.LAYER_PAWN + 0.01f);
            //Add hair if this head uses hair and doesn't have hair hidden.
            if (HasHair && (Body.Parent.HiddenBodypartsFlags & ClothingFlags.HIDE_HAIR) == 0)
                renderable.AddOverlay($"hair", new StandardRenderable($"human_face.{haircut}"), Layers.LAYER_PAWN + 0.04f);
        }

        public override void RemoveOverlay(Renderable renderable)
        {
            renderable.RemoveOvelay($"head");
            renderable.RemoveOvelay($"hair");
        }

        public override void UpdateDamageOverlays(Renderable renderable)
        {
            if (renderable.HasOverlay($"damhead"))
                renderable.RemoveOvelay($"damhead");
            if (Health <= 0)
                renderable.AddOverlay($"damhead", new StandardRenderable($"brute_head_2"), Layers.LAYER_PAWN + 0.03f);
            else if (Health < MaxHealth)
                renderable.AddOverlay("damhead", new StandardRenderable($"brute_head_0"), Layers.LAYER_PAWN + 0.03f);
        }

        public override void UpdateCoveredOverlays(Renderable renderable, ClothingFlags oldFlags, ClothingFlags newFlags)
        {
            //Check if the visability of hair changed
            if ((newFlags & ClothingFlags.HIDE_HAIR) != (oldFlags & ClothingFlags.HIDE_HAIR))
            {
                if ((newFlags & ClothingFlags.HIDE_HAIR) == 0)
                {
                    //Hair is now visible
                    renderable.AddOverlay($"hair", new StandardRenderable($"human_face.{haircut}"), Layers.LAYER_PAWN + 0.04f);
                }
                else
                {
                    //Hair is no longer visible
                    renderable.RemoveOvelay($"hair");
                }
            }
        }
    }
}
