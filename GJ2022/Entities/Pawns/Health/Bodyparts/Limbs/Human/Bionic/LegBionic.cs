using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Human.Bionic
{
    public class LegBionic : LegHuman
    {
        public LegBionic(Body body, BodySlots slot) : base(body, slot)
        {
        }

        public override float MaxHealth => 50;

        public override float MovementFactor => 65;

        public override float HighPressureDamage => 300;

        public override float LowPressureDamage => 0;

        public override void AddOverlay(Renderable renderable)
        {
            string direction = InsertedSlot == BodySlots.SLOT_LEG_LEFT ? "left" : "right";
            renderable.AddOverlay($"{direction}leg", new StandardRenderable($"bionic_{direction}leg"), Layers.LAYER_PAWN + 0.01f);
        }
    }
}
