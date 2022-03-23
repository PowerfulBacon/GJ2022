using GJ2022.EntityLoading;
using GJ2022.EntityLoading.XmlDataStructures;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Game.Construction
{
    public class BlueprintCategory : IInstantiatable
    {

        /// <summary>
        /// The name of the category
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The set of blueprints contained by this category
        /// </summary>
        public IEnumerable<BlueprintData> Blueprints { get; private set; }

        public EntityDef TypeDef { get; set; }

        public void Initialize(Vector<float> initializePosition)
        { }

        public void PreInitialize(Vector<float> initializePosition)
        { }

        public void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "Name":
                    Name = (string)property;
                    return;
                case "Blueprints":
                    Blueprints = ((List<object>)property).OfType<BlueprintData>();
                    return;
            }
            throw new NotImplementedException($"Unrecognised property {name}");
        }
    }
}
