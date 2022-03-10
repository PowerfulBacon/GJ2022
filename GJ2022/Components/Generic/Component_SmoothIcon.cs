using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Generic
{
    public class Component_SmoothIcon : Component
    {

        public string Key { get; private set; }

        public override void OnComponentAdd()
        {
            
        }

        public override void OnComponentRemove()
        {
            
        }

        public override void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "Key":
                    Key = (string)property;
                    return;
            }
            throw new NotImplementedException($"Unknown property {name}.");
        }

    }
}
