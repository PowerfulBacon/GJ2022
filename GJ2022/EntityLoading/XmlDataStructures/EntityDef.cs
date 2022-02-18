﻿using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GJ2022.EntityLoading.XmlDataStructures
{
    public class EntityDef : PropertyDef
    {
        public EntityDef(string name) : base(name)
        {
        }

        public override object GetValue(params object[] ctorParams)
        {
            if (Tags.ContainsKey("Abstract"))
                throw new Exception("Cannot instantiate an abstract class");
            if (!Tags.ContainsKey("Class"))
                throw new XmlException($"Property with name {Name} did not have the required tag Class.");
            //Get the class to instantiate
            string className = Tags["Class"];
            //Locate the type to load
            if (!EntityConfig.ClassTypeCache.ContainsKey(className))
                throw new XmlException($"Property with name {Name} has an invalid Class tag ({className} is not a known class)");
            Type loadedType = EntityConfig.ClassTypeCache[className];
            //Instantiate the located type
            //Locate the first constructor
            ConstructorInfo firstConstructor = loadedType.GetConstructors()[0];
            //Invoke the first constructor located, either with the provided constructor parameters or nothing
            IInstantiatable created = firstConstructor.Invoke(firstConstructor.GetParameters().Length > 0 ? ctorParams : new object[0]) as IInstantiatable;
            //The created class is not an instantiatable type
            if (created == null)
            {
                throw new XmlException($"The class {className} is not an instantiatable type. It must implement IInstantiable.");
            }
            //Set the properties of this object
            foreach (PropertyDef property in GetChildren())
            {
                object propertyValue = null;
                try
                {
                    propertyValue = property.GetValue(ctorParams);
                    created.SetProperty(property.Name, propertyValue);
                }
                catch (Exception e)
                {
                    Log.WriteLine($"{e}\nProperty Name: {property.Name}\nProperty Value: {propertyValue.ToString() ?? "NULL REFERENCE"}", LogType.ERROR);
                }
            }
            //Return the created thing
            return created;
        }

        public object Instantiate()
        {
            return GetValue();
        }

        /// <summary>
        /// Instantiate the provided entity at the given position
        /// </summary>
        public object InstantiateAt(Vector<float> position)
        {
            return GetValue(position);
        }

    }
}
