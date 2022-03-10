using GJ2022.Entities.Pawns;
using GJ2022.EntityLoading;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Atmospherics
{
    public class Component_Jetpack : Component_GasStorage
    {

        protected override object OnEquipped(object source, params object[] data)
        {
            Pawn pawn = (Pawn)data[0];
            pawn.RegisterSignal(Signal.SIGNAL_ENTITY_MOVED, -1, MakeSparkles);
            return base.OnEquipped(source, data);
        }

        protected override object OnUnequipped(object source, params object[] data)
        {
            Pawn pawn = (Pawn)data[0];
            pawn.UnregisterSignal(Signal.SIGNAL_ENTITY_MOVED, MakeSparkles);
            return base.OnUnequipped(source, data);
        }

        private object MakeSparkles(object source, params object[] arguments)
        {
            Pawn pawn = (Pawn)source;
            EntityCreator.CreateEntity("Sparkle", (Vector<int>)pawn.Position);
            return null;
        }

        public override void SetProperty(string name, object property)
        {
            base.SetProperty(name, property);
        }
    }
}
