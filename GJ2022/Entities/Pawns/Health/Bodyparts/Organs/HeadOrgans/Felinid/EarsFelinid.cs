using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs.HeadOrgans.Felinid
{
    public class EarsFelinid : Ear
    {
        public EarsFelinid(Pawn parent, Body body) : base(parent, body)
        {
        }

        public override float HearingFactor => 55;

        public override float MaxHealth => 5;

        public override void AddOverlay(Renderable renderable)
        {
            renderable.AddOverlay($"ears_front", new StandardRenderable($"mutant_bodyparts.m_ears_cat_FRONT"), Layers.LAYER_PAWN + 0.035f);
            renderable.AddOverlay($"ears_behind", new StandardRenderable($"mutant_bodyparts.m_ears_cat_BEHIND"), Layers.LAYER_PAWN + 0.033f);
        }

        public override void RemoveOverlay(Renderable renderable)
        {
            renderable.RemoveOvelay($"ears_front");
            renderable.RemoveOvelay($"ears_behind");
        }
    }
}
