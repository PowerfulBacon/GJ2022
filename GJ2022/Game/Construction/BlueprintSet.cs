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
    public class BlueprintSet : IInstantiatable
    {

        /// <summary>
        /// The name of the blueprint set.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The construction mode of this blueprint set.
        /// Defines how it is made in the world.
        /// </summary>
        public BlueprintMode Mode { get; private set; }

        /// <summary>
        /// The blueprint data object of the thing being created
        /// </summary>
        public BlueprintData CreatedBlueprint { get; private set; }

        /// <summary>
        /// The blueprint to build in the borders.
        /// Required only if the Mode is BlueprintMode.FILL_BOX.
        /// All other modes use CreatedBlueprint only.
        /// </summary>
        public BlueprintData FilledBoxBorderBlueprint { get; private set; }

        /// <summary>
        /// The blueprint event executed when this set is selected.
        /// </summary>
        public IBlueprintEvent BlueprintEvent { get; private set; }

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
                case "Mode":
                    Mode = (BlueprintMode)property;
                    return;
                case "CreatedBlueprint":
                    CreatedBlueprint = (BlueprintData)property;
                    return;
                case "FilledBoxBorderBlueprint":
                    FilledBoxBorderBlueprint = (BlueprintData)property;
                    return;
                case "BlueprintEvent":
                    BlueprintEvent = (IBlueprintEvent)property;
                    return;
            }
            throw new NotImplementedException($"Invalid property: {name}");
        }
    }
}
