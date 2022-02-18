using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.EntityLoading.XmlDataStructures
{
    public class NumericalDef : PropertyDef
    {

        private double value;

        public NumericalDef(string name, string value) : base(name)
        {
            this.value = double.Parse(value);
        }

        public override object GetValue(params object[] ctorParams)
        {
            return value;
        }

        protected override void UpdateFrom(PropertyDef overrider)
        {
            value = ((NumericalDef)overrider).value;
            base.UpdateFrom(overrider);
        }
    }
}
