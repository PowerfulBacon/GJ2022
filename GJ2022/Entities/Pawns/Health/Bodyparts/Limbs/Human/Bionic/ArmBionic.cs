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
    class ArmBionic : ArmHuman
    {

        public ArmBionic(Body body, BodySlots slot) : base(body, slot)
        { }

        //15 more than a regular arm
        public override float MaxHealth => 50;

        //15 more manipulation.
        public override float ManipulationFactor => 65;

        public override void AddOverlay(Renderable renderable)
        {
            string direction = InsertedSlot == BodySlots.SLOT_ARM_LEFT ? "left" : "right";
            renderable.AddOverlay($"{direction}arm", new StandardRenderable($"bionic_{direction}arm"), Layers.LAYER_PAWN + 0.01f);
            renderable.AddOverlay($"{direction}hand", new StandardRenderable($"bionic_{direction}hand"), Layers.LAYER_PAWN + 0.02f);
        }
    }
}
