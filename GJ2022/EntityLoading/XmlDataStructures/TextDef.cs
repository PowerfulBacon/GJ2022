using GJ2022.Utility.MathConstructs;
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

        public override object GetValue(Vector<float> initializePosition)
        {
            return value;
        }

        protected override void UpdateFrom(PropertyDef overrider)
        {
            value = ((TextDef)overrider).value;
            base.UpdateFrom(overrider);
        }

        public override PropertyDef Copy()
        {
            TextDef copy = new TextDef(Name, value);
            foreach (string key in Tags.Keys)
                copy.Tags.Add(key, Tags[key]);
            foreach (string key in Children.Keys)
                copy.Children.Add(key, Children[key].Copy());
            return copy;
        }
    }
}
