using GJ2022.EntityLoading.XmlDataStructures;
using GJ2022.PawnBehaviours.InteractionEvents;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Generic
{
    /// <summary>
    /// A generic provider for pawn interactions with this object.
    /// </summary>
    public class Component_Interaction : Component_Tracked
    {

        //The name of the AI helper for this interaction.
        //Allows AI behaviours to locate interactions that it wants
        public string AiHelperName { get; private set; } = null;

        //The event that will occur when a pawn interacts with this
        //component.
        public IInteractionEvent InteractionEvent { get; private set; }

        public override void OnComponentAdd()
        {
            base.OnComponentAdd();
        }

        public override void OnComponentRemove()
        {
            base.OnComponentRemove();
        }

        public override void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "AiHelperName":
                    AiHelperName = (string)property;
                    //Set the key of the tracked element to the AiHelperName
                    base.SetProperty("Key", property);
                    return;
                case "InteractionEvent":
                    InteractionEvent = (IInteractionEvent)property;
                    return;
            }
            base.SetProperty(name, property);
        }

    }
}
