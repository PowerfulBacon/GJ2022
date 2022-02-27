using GJ2022.Components.Generic;
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
        }

        public override void OnComponentRemove()
        {
            Parent.RemoveComponent(aiHelperTracker);
            aiHelperTracker = null;
        }

        public override void SetProperty(string name, object property)
        {
            throw new NotImplementedException();
        }
    }
}
