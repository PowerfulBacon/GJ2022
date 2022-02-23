using GJ2022.Entities;
using GJ2022.EntityLoading.XmlDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Generic
{
    public class Component_Mineable : Component
    {

        public EntityDef DroppedItemDef { set; private get; } = null;

        public int MinDropCount { set; private get; } = 1;

        public int MaxDropCount { set; private get; } = 1;

        public float DropProbability { set; private get; } = 1;

        public override void OnComponentAdd()
        {
            Parent.RegisterSignal(Signal.SIGNAL_ENTITY_MINE, -1, OnMineEvent);
        }

        public override void OnComponentRemove()
        {
            Parent.UnregisterSignal(Signal.SIGNAL_ENTITY_MINE, OnMineEvent);
        }

        private object OnMineEvent(object source, params object[] arguments)
        {
            Entity parentEntity = Parent as Entity;
            //Drop item
            DroppedItemDef.GetValue(parentEntity.Position);
            //Destroy parent
            parentEntity.Destroy();
            return null;
        }

        public override void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "DroppedItemDef":
                    DroppedItemDef = (EntityDef)property;
                    return;
                case "MinDropCount":
                    MinDropCount = Convert.ToInt32(property);
                    return;
                case "MaxDropCount":
                    MaxDropCount = Convert.ToInt32(property);
                    return;
                case "DropProbability":
                    DropProbability = Convert.ToSingle(property);
                    return;
            }
            throw new NotImplementedException($"Invalid property {name}");
        }

    }
}
