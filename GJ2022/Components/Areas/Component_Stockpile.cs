using GJ2022.Components.Generic;
using GJ2022.Entities;
using GJ2022.Entities.Items;
using GJ2022.Managers.Stockpile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Areas
{
    public class Component_Stockpile : Component
    {

        Component_Tracked aiHelperTracker;

        public override void OnComponentAdd()
        {
            //Automatically add in the tracked component
            aiHelperTracker = new Component_Tracked();
            aiHelperTracker.Key = "Stockpile";
            Parent.AddComponent(aiHelperTracker);
            //Add stockpile area
            Entity parent = Parent as Entity;
            StockpileManager.AddStockpileArea(parent.Position);
            //Register signals
            Parent.RegisterSignal(Signal.SIGNAL_AREA_CONTENTS_ADDED, -1, AddContents);
            Parent.RegisterSignal(Signal.SIGNAL_AREA_CONTENTS_REMOVED, -1, AddContents);
        }

        public override void OnComponentRemove()
        {
            Parent.RemoveComponent(aiHelperTracker);
            aiHelperTracker = null;
            //Remove stockpiler area
            Entity parent = Parent as Entity;
            StockpileManager.RemoveStockpileArea(parent.Position);
            //Unregister signals
            Parent.UnregisterSignal(Signal.SIGNAL_AREA_CONTENTS_ADDED, AddContents);
            Parent.UnregisterSignal(Signal.SIGNAL_AREA_CONTENTS_REMOVED, RemoveContents);
        }

        private object AddContents(object source, params object[] parameters)
        {
            Item item = parameters[0] as Item;
            StockpileManager.AddItem(item);
            return null;
        }

        private object RemoveContents(object source, params object[] parameters)
        {
            Item item = parameters[0] as Item;
            StockpileManager.RemoveItem(item);
            return null;
        }

        public override void SetProperty(string name, object property)
        {
            throw new NotImplementedException();
        }
    }
}
