using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs.Human
{
    public class LegHuman : Limb
    {
        public override BodySlots[] AllowedSlots => new BodySlots[] { BodySlots.SLOT_LEG_LEFT, BodySlots.SLOT_LEG_RIGHT };

        public override LimbFlags DefaultLimbFlags => LimbFlags.NONE;
    }
}
