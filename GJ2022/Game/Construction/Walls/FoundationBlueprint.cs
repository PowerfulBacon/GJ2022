using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Game.Construction.Walls
{
    public class FoundationBlueprint : BlueprintDetail
    {

        public override string Name => "Metal Foundations";

        public override bool IsRoom => true;

        public override Type BorderType => typeof(Entities.Debug.DebugEntity);

        public override Type FloorType => typeof(Entities.Debug.DebugEntity);

    }
}
