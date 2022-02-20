using GJ2022.Utility.MathConstructs;
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
            this.value = double.Parse(value.Trim());
        }

        public override object GetValue(Vector<float> initializePosition)
        {
            return value;
        }

        protected override void UpdateFrom(PropertyDef overrider)
        {
            value = ((NumericalDef)overrider).value;
            base.UpdateFrom(overrider);
        }

        public override PropertyDef Copy()
        {
            NumericalDef copy = new NumericalDef(Name, value.ToString());
            foreach (string key in Tags.Keys)
                copy.Tags.Add(key, Tags[key]);
            foreach (string key in Children.Keys)
                copy.Children.Add(key, Children[key].Copy());
            return copy;
        }
    }
}
