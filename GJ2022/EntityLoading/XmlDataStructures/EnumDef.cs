﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.EntityLoading.XmlDataStructures
{
    public class EnumDef : PropertyDef
    {

        public EnumDef(string name) : base(name)
        { }

        public override object GetValue(Vector<float> initializePosition)
        {
            return base.GetValue(initializePosition);
        }

        public T GetValue<T>()
            where T : Enum
        {
            int value = 0;
            foreach (PropertyDef property in GetChildren())
            {
                value |= (int)Enum.Parse(typeof(T), (string)property.GetValue(Vector<float>.Zero));
            }
            return (T)Enum.ToObject(typeof(T), value);
        }

    }
}
