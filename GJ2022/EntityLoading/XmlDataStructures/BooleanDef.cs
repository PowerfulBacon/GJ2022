using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GJ2022.EntityLoading.XmlDataStructures
{
    public class BooleanDef : PropertyDef
    {

        private bool value;

        public BooleanDef(string name, string value) : base(name)
        {
            this.value = value == "true" ? true : value == "false" ? false : throw new XmlException($"Unable to parse boolean value of {value}.");
        }

        public override object GetValue(params object[] ctorParams)
        {
            return value;
        }

        protected override void UpdateFrom(PropertyDef overrider)
        {
            value = ((BooleanDef)overrider).value;
            base.UpdateFrom(overrider);
        }
    }
}
