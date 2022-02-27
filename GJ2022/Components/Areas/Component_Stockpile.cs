using GJ2022.Components.Generic;
using GJ2022.Entities;
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
        }

        public override void OnComponentRemove()
        {
            Parent.RemoveComponent(aiHelperTracker);
            aiHelperTracker = null;
            //Remove stockpiler area
            Entity parent = Parent as Entity;
            StockpileManager.RemoveStockpileArea(parent.Position);
        }

        public override void SetProperty(string name, object property)
        {
            throw new NotImplementedException();
        }
    }
}
