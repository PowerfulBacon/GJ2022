using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Rendering.RenderSystems.Renderables;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Limbs.Corgi
{
    class HeadCorgi : HeadHuman
    {
        public HeadCorgi(Body body, BodySlots slot) : base(body, slot)
        {
        }

        public override float MaxHealth => 15;

        public override LimbFlags DefaultLimbFlags => LimbFlags.CRITICAL_LIMB | LimbFlags.NO_INSERTION;

    }
}
