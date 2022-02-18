using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.EntityLoading.XmlDataStructures
{
    public class TextDef : PropertyDef
    {

        private string value;

        public TextDef(string name, string value) : base(name)
        {
            this.value = value;
        }

        public override object GetValue(params object[] ctorParams)
        {
            return value;
        }

        protected override void UpdateFrom(PropertyDef overrider)
        {
            value = ((TextDef)overrider).value;
            base.UpdateFrom(overrider);
        }
    }
}
