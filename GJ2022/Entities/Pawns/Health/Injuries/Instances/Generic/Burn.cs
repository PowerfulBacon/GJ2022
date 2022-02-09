using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Injuries.Instances.Generic
{
    public class Burn : Injury
    {
        public Burn(float damage) : base(damage)
        {
        }

        public override string Name => "Burn";

        public override bool Unique => false;

        public override float PainPerDamage => 1.3f;
    }
}
