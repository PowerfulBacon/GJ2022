using GJ2022.Entities;
using GJ2022.EntityLoading.XmlDataStructures;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Items
{
    /// <summary>
    /// A component that creates an entity and stores it inside the parent.
    /// </summary>
    public class Component_InternalEntity : Component
    {

        /// <summary>
        /// The definition of the entity to create when the component is added
        /// </summary>
        public EntityDef StoredEntityDef { get; set; }

        /// <summary>
        /// The entity stored within us
        /// </summary>
        protected Entity StoredEntity { get; set; }

        public override void OnComponentAdd()
        {
            //Instantiate the entity
            StoredEntity = (Entity)StoredEntityDef.GetValue(Vector<float>.Zero);
            //Move it inside our parent
            StoredEntity.Location = Parent as Entity;
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
                case "StoredEntity":
                    //Instantiate the entity
                    StoredEntityDef = ((EntityDef)property);
                    return;
            }
            throw new NotImplementedException();
        }
    }
}
