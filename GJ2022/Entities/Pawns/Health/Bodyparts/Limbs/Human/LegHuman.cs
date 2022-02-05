﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Human
{
    public class LegHuman : Limb
    {
        public LegHuman(Body body, BodySlots slot) : base(body, slot)
        {
        }

        public override BodySlots[] AllowedSlots => new BodySlots[] { BodySlots.SLOT_LEG_LEFT, BodySlots.SLOT_LEG_RIGHT };

        public override LimbFlags DefaultLimbFlags => LimbFlags.NONE;

        public override float MaxHealth => 35;

        public override float MovementFactor => 50;

        public override void SetupOrgans(Pawn pawn, Body body)
        {
            //TODO
            return;
        }

        public override void AddOverlay(Renderable renderable)
        {
            string direction = InsertedSlot == BodySlots.SLOT_LEG_LEFT ? "left" : "right";
            renderable.AddOverlay($"{direction}leg", new StandardRenderable($"human_{direction}leg"), Layers.LAYER_PAWN + 0.01f);
        }

        public override void RemoveOverlay(Renderable renderable)
        {
            string direction = InsertedSlot == BodySlots.SLOT_LEG_LEFT ? "left" : "right";
            renderable.RemoveOvelay($"{direction}leg");
        }
    }
}
