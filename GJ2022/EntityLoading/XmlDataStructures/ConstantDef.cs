using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.EntityLoading.XmlDataStructures
{
    public class ConstantDef : PropertyDef
    {

        public ConstantDef(string name) : base(name)
        { }

        public override object GetValue(Vector<float> initializePosition)
        {
            return EntityConfig.LoadedConstants[GetChild("Constant").Tags["Name"]].GetValue(initializePosition);
        }

    }
}
