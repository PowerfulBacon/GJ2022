using GJ2022.Entities;
using GJ2022.Subsystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Bespoke
{
    public class Component_Blueprint : Component
    {

        public override void OnComponentAdd()
        { }

        public override void OnComponentRemove()
        {
            //Upon deletion, remove ourself from the blueprint system
            BlueprintSystem.Singleton.DeleteBlueprintData(Parent as Entity);
        }

        public override void SetProperty(string name, object property)
        {
            throw new NotImplementedException();
        }

    }
}
