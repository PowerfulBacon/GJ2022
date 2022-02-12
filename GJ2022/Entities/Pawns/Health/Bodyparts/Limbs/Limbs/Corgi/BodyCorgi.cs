using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Rendering.RenderSystems.Renderables;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Limbs.Corgi
{
    public class BodyCorgi : BodyHuman
    {
        public BodyCorgi(Body body, BodySlots slot) : base(body, slot)
        {
        }

        public override float MaxHealth => 25;

        public override void AddOverlay(Renderable renderable)
        {
            return;
        }

        public override void RemoveOverlay(Renderable renderable)
        {
            return;
        }

        public override void UpdateDamageOverlays(Renderable renderable)
        {
            return;
        }
    }
}
