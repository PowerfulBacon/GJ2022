using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodies.Instances
{
    public class BodyDog : Body
    {
        public override BodySlots[] BodySlots => new BodySlots[] {
            Health.BodySlots.SLOT_BODY,
            Health.BodySlots.SLOT_HEAD,
            Health.BodySlots.SLOT_TAIL,
            Health.BodySlots.SLOT_PAW_BACK_LEFT,
            Health.BodySlots.SLOT_PAW_BACK_RIGHT,
            Health.BodySlots.SLOT_PAW_LEFT,
            Health.BodySlots.SLOT_PAW_RIGHT,
            Health.BodySlots.SLOT_LEG_BACK_LEFT,
            Health.BodySlots.SLOT_LEG_BACK_RIGHT,
            Health.BodySlots.SLOT_LEG_FRONT_LEFT,
            Health.BodySlots.SLOT_LEG_FRONT_RIGHT,
        };

        protected override void CreateDefaultBodyparts()
        {
            //TODO
            return;
        }
    }
}
