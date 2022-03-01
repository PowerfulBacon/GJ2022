using GJ2022.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GJ2022.Components.Generic
{
    public class Component_DeleteAfter : Component
    {

        public int DeleteTime { get; private set; }

        public override void OnComponentAdd()
        {
            Task.Run(() => {
                Thread.Sleep(DeleteTime);
                if (Parent == null)
                    return;
                Entity e = Parent as Entity;
                e.Destroy();
            });
        }

        public override void OnComponentRemove()
        { }

        public override void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "DeleteTime":
                    DeleteTime = Convert.ToInt32(property);
                    return;
            }
            throw new NotImplementedException($"Invalid property {name}");
        }

    }
}
