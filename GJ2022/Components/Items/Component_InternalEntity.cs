using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Items
{
    public class Component_InternalEntity : Component
    {

        public string StoredEntityName { get; set; }

        public Entity StoredEntity { get; set; }

        public override void OnComponentAdd()
        {
            //Create the stored entity

            throw new NotImplementedException();
        }

        public override void OnComponentRemove()
        {
            //Destroy the stored entity
            throw new NotImplementedException();
        }

        public override void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "StoredEntityName":
                    StoredEntityName = (string)property;
                    return;
            }
            throw new NotImplementedException();
        }
    }
}
