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
    public class BlueprintData : IInstantiatable
    {

        /// <summary>
        /// The entity def that will be created upon completion of this blueprint
        /// </summary>
        public EntityDef CreatedDef { get; private set; }

        /// <summary>
        /// The materials required to create this blueprint
        /// </summary>
        public ConstructionCost ConstructionCost { get; private set; } = ConstructionCost.Free;

        /// <summary>
        /// The layer that the blueprint will be placed on
        /// </summary>
        public int Layer { get; private set; }

        /// <summary>
        /// The priority of the blueprint.
        /// Blueprints cannot be placed in a location where there is another blueprint
        /// on the same layer with a higher priority.
        /// </summary>
        public int Priority { get; private set; } = 0;

        /// <summary>
        /// The amount of work it takes to complete this blueprint.
        /// </summary>
        public int WorkAmount { get; private set; } = 0;

        //Required property for instantiatable types.
        public EntityDef TypeDef { get; set; }

        public void Initialize(Vector<float> initializePosition)
        { }

        public void PreInitialize(Vector<float> initializePosition)
        { }

        public void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "CreatedDef":
                    //TODO: Refactor TypeDef loading to support EntityDef typed properties.
                    CreatedDef = (EntityDef)property;
                    return;
                case "ConstructionCost":
                    ConstructionCost = (ConstructionCost)property;
                    return;
                case "Layer":
                    Layer = Convert.ToInt32(property);
                    return;
                case "Priority":
                    Priority = Convert.ToInt32(property);
                    return;
                case "WorkAmount":
                    WorkAmount = Convert.ToInt32(WorkAmount);
                    return;
            }
            throw new NotImplementedException($"{name} is not implemented.");
        }
    }
}
