using GJ2022.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Generic
{
    public class Component_Tracked : Component
    {

        public string Key { get; set; }

        private int trackedX;
        private int trackedY;

        public override void OnComponentAdd()
        {
            Entity parent = Parent as Entity;
            //Start tracking
            trackedX = (int)parent.Position[0];
            trackedY = (int)parent.Position[1];
        }

        public override void OnComponentRemove()
        {
            Entity parent = Parent as Entity;
            //Stop tracking
        }

        public override void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "Key":
                    Key = (string)property;
                    return;
            }
            throw new NotImplementedException();
        }
    }
}
