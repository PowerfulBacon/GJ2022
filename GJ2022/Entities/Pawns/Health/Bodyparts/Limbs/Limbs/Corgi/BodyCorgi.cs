﻿using GJ2022.Entities.Pawns.Health.Bodies;
using GJ2022.Rendering.RenderSystems.Renderables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
