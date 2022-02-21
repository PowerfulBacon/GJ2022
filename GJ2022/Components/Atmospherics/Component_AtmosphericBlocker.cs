using GJ2022.Entities;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Atmospherics
{
    public class Component_AtmosphericBlocker : Component
    {

        private Vector<int> blockingPosition;

        public override void OnComponentAdd()
        {
            Entity parent = Parent as Entity;
            blockingPosition = parent.Position;
            Log.WriteLine($"{blockingPosition} : {parent.Position}", LogType.TEMP);
            World.AddAtmosphericBlocker(blockingPosition[0], blockingPosition[1]);
            Parent.RegisterSignal(Signal.SIGNAL_ENTITY_MOVED, -1, ParentMoveReact);
        }

        public override void OnComponentRemove()
        {
            Log.WriteLine($"REMOVED: {blockingPosition}", LogType.TEMP);
            World.RemoveAtmosphericBlock(blockingPosition[0], blockingPosition[1]);
            Parent.UnregisterSignal(Signal.SIGNAL_ENTITY_MOVED, ParentMoveReact);
        }

        private object ParentMoveReact(object source, params object[] parameters)
        {
            World.RemoveAtmosphericBlock(blockingPosition[0], blockingPosition[1]);
            Entity parent = Parent as Entity;
            blockingPosition = parent.Position;
            Log.WriteLine($"MOVED: {blockingPosition}", LogType.TEMP);
            World.AddAtmosphericBlocker(blockingPosition[0], blockingPosition[1]);
            return null;
        }

        public override void SetProperty(string name, object property)
        {
            throw new NotImplementedException();
        }

    }
}
