﻿using GJ2022.EntityLoading.XmlDataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GJ2022.EntityLoading
{
    public static class EntityLoader
    {

        private enum DefTypes
        {
            Entity,         //Entity: Top level entity (Used in the same way as class, but stored in the entityDef list)
            Numerical,      //A numerical value, floating point or integer
            Boolean,        //A boolean value, true or false
            Text,           //A text value
            Enumerator,     //An enumerator value, type is infered from the property name
            Class,          //A class, when the value is got the class is instantiated and the properties defined in children xml nodes are set
            List,           //A simple list, has <ListEntry> for children
            Dictionary,     //A key-value pair with <DictEntry><Key>...</Key><Value>...</Value>
            Constant,       //A constant value loaded from a constant list
            Components,     //Components is essentially a list but with the elements at the top level
            Property,       //All else failed
        }

        private const string GAME_DATA_DIRECTORY = "Data/GameData";

        /// <summary>
        /// Load the entity XML file.
        /// </summary>
        public static void LoadEntities()
        {
            //Clear existing data
            if (EntityConfig.LoadedEntityDefs.Count > 0)
            {
                Log.WriteLine("Clearing existing EntityConfig data... (LoadEntities was called again, likely a debug call)", LogType.WARNING);
                EntityConfig.LoadedEntityDefs.Clear();
            }
            //Locate all game data XML files.
            foreach (string gameDataGroupName in Directory.GetDirectories(GAME_DATA_DIRECTORY))
            {
                //Locate the XML files at this location
                string[] locatedFiles = Directory.GetFiles(gameDataGroupName, "*.xml", SearchOption.AllDirectories);
                //Log the amount of files we are loading
                Log.WriteLine($"Loading {gameDataGroupName} ({locatedFiles.Length} files located)", LogType.LOG);
                //Load the files
                foreach (string xmlFile in locatedFiles)
                {
                    LoadEntityFile(xmlFile);
                }
            }
            //Parse anything that needs reparsing (Parents weren't loaded originally)
            int prevLength = queuedDefs.Count;
            int failureCount = queuedDefs.Count + 1;
            while (queuedDefs.Count > 0 && failureCount > 0)
            {
                //Sanity (Crash if someone creats a fake parent)
                if (prevLength != queuedDefs.Count)
                {
                    prevLength = queuedDefs.Count;
                    failureCount = queuedDefs.Count + 1;
                }
                //Load
                (PropertyDef, XElement, PropertyDef) todo = queuedDefs.Dequeue();
                //If dependancy isn't loaded (its either in this queue or doesn't exist), send to the back of the queue
                if (!todo.Item1.DependanciesLoaded())
                    queuedDefs.Enqueue(todo);
                else
                {
                    try
                    {
                        //Dependancy now exists, so parse as usual
                        ContinueParse(todo.Item1, todo.Item2, todo.Item3);
                    }
                    catch (XmlException xmlException)
                    {
                        Log.WriteLine(xmlException, LogType.ERROR);
                    }
                }
                //Ensure success
                failureCount--;
            }
            if (failureCount == 0)
            {
                Log.WriteLine($"ERROR: Recursive depandancy loading detected, {queuedDefs.Count} items have invalid parents", LogType.ERROR);
                foreach ((PropertyDef, XElement, PropertyDef) error in queuedDefs.ToList())
                {
                    Log.WriteLine($"Cannot locate the parent of {(error.Item1.Tags.ContainsKey("Name") ? error.Item1.Tags["Name"] : error.Item1.Name)}", LogType.ERROR);
                }
            }
            //Create a log message
            Log.WriteLine($"Loaded {EntityConfig.LoadedEntityDefs.Count} entities...", LogType.LOG);
        }

        private static Queue<(PropertyDef, XElement, PropertyDef)> queuedDefs = new Queue<(PropertyDef, XElement, PropertyDef)>();

        private static void LoadEntityFile(string file)
        {
            //Load the XML file
            XElement entitiesElement = XElement.Load(file);
            //Begin parsing
            foreach (XElement element in entitiesElement.Elements())
            {
                try
                {
                    RecursivelyParse(element);
                }
                catch (XmlException xmlException)
                {
                    Log.WriteLine(xmlException, LogType.ERROR);
                }
            }
        }

        private static void RecursivelyParse(XElement element, PropertyDef parentProperty = null)
        {
            PropertyDef createdProperty;
            switch (IdentifyDefinitionType(element))
            {
                case DefTypes.Class:
                case DefTypes.Entity:
                    createdProperty = new EntityDef(element.Name.LocalName);
                    break;
                case DefTypes.Boolean:
                    createdProperty = new BooleanDef(element.Name.LocalName, element.Value);
                    break;
                case DefTypes.Numerical:
                    createdProperty = new NumericalDef(element.Name.LocalName, element.Value);
                    break;
                case DefTypes.Text:
                    createdProperty = new TextDef(element.Name.LocalName, element.Value);
                    break;
                case DefTypes.Property:
                    createdProperty = new PropertyDef(element.Name.LocalName);
                    break;
                case DefTypes.List:
                    createdProperty = new ListDef(element.Name.LocalName);
                    break;
                default:
                    throw new XmlException($"Unable to instantiate defType {IdentifyDefinitionType(element)}.");
            }
            //Set our tags
            foreach (XAttribute tag in element.Attributes())
            {
                createdProperty.AddTag(tag.Name.LocalName, tag.Value);
            }
            //Check if we can be loaded
            if (!createdProperty.DependanciesLoaded())
            {
                Log.WriteLine($"{(createdProperty.Tags.ContainsKey("Name") ? createdProperty.Tags["Name"] : createdProperty.Name)} has unloaded dependancies, enqueuing.");
                queuedDefs.Enqueue((createdProperty, element, parentProperty));
                return;
            }
            ContinueParse(createdProperty, element, parentProperty);
        }

        private static void ContinueParse(PropertyDef createdProperty, XElement element, PropertyDef parentProperty)
        {
            //Inerhit dependancy properties
            createdProperty.InheritFromDependancy();
            //Set our created properties things up
            foreach (XElement childElement in element.Elements())
            {
                try
                {
                    RecursivelyParse(childElement, createdProperty);
                }
                catch (XmlException xmlException)
                {
                    Log.WriteLine(xmlException, LogType.ERROR);
                }
            }
            //Add as property
            if (parentProperty != null)
            {
                parentProperty.AddChild(createdProperty);
            }
            else
            {
                //Add to the list of EntityDefs or ConstDefs or whatever
                Log.WriteLine($"Successfully created {(createdProperty.Tags.ContainsKey("Name") ? createdProperty.Tags["Name"] : createdProperty.Name)}");
                if (createdProperty is EntityDef)
                {
                    EntityConfig.LoadedEntityDefs.Add(createdProperty.Tags["Name"], createdProperty as EntityDef);
                }
            }
        }

        private static DefTypes IdentifyDefinitionType(XElement element)
        {
            if (element.Name.Equals(XName.Get("Constant")))
                return DefTypes.Constant;
            if (element.Attribute(XName.Get("Name")) != null)
                return DefTypes.Entity;
            if (element.Attribute(XName.Get("Class")) != null)
                return DefTypes.Class;
            if (element.HasElements)
            {
                if (element.Element(XName.Get("ListEntry")) != null)
                    return DefTypes.List;
                if (element.Element(XName.Get("DictEntry")) != null)
                    return DefTypes.Dictionary;
                if (element.Element(XName.Get("EnumValue")) != null)
                    return DefTypes.Enumerator;
                return DefTypes.Property;
            }
            if (element.Value == "true" || element.Value == "false")
                return DefTypes.Boolean;
            if (long.TryParse(element.Value, out _))
                return DefTypes.Numerical;
            return DefTypes.Text;
        }

    }
}
