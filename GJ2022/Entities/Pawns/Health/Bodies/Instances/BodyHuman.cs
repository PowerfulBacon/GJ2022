using GJ2022.Entities.Pawns.Health.Bodyparts.Limbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodies.Instances
{
    public class BodyHuman : Body
    {
        public override BodySlots[] BodySlots => throw new NotImplementedException();

        protected override void CreateDefaultBodyparts()
        {
            new Bodyparts.Limbs.Human.BodyHuman().SetupOrgans(Parent, this);
        }
    }
}
