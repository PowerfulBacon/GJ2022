using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GJ2022.EntityLoading.XmlDataStructures
{
    public class ListDef : PropertyDef
    {

        public new List<PropertyDef> Children { get; } = new List<PropertyDef>();

        public ListDef(string name) : base(name)
        { }

        public override void AddChild(PropertyDef property)
        {
            Children.Add(property);
            if (property.Tags.ContainsKey("Override"))
                throw new XmlException("The override tag cannot be used on lists!");
        }

        public override object GetValue(params object[] ctorParams)
        {
            List<object> values = new List<object>();
            foreach (PropertyDef property in GetChildren())
            {
                values.Add(property.GetValue(ctorParams));
            }
            return values;
        }

        public override IReadOnlyCollection<PropertyDef> GetChildren()
        {
            return Children;
        }

        /// <summary>
        /// Merge lists by getting elements from all
        /// </summary>
        protected override void UpdateFrom(PropertyDef overrider)
        {
            //Update all incoming properties
            foreach (PropertyDef incomingProperty in overrider.GetChildren())
            {
                AddChild(incomingProperty);
            }
        }

    }
}
