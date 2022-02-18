using GJ2022.Atmospherics;
using GJ2022.Entities.Pawns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Atmospherics
{
    public class Component_GasStorage : Component
    {

        //The atmosphere contained within this gas storage component
        private Atmosphere containedAtmopshere;

        //===Component Properties===

        /// <summary>
        /// The creator for the default atmosphere of the gas storage.
        /// The contained atmosphere of this component will be generated based
        /// on the values assigned in this property.
        /// </summary>
        /// <usage>
        /// <DefaultGas Class="AtmosphereCreator">
        ///   <KelvinTemperature>
        ///     <Constant Name="IDEAL_TEMPERATURE"/>
        ///   </KelvinTemperature>
        ///   <LitreVolume> 40 </LitreVolume>
        ///   <Oxygen>12</Oxygen>
        ///   <Hydrogen>600</Hydrogen>
        ///   <CarbonDioxide>4.6</CarbonDioxide>
        /// </DefaultGas >
        /// </usage>
        public AtmosphereCreator DefaultGas { private get; set; }

        /// <summary>
        /// Is this gas storage a valid internals source.
        /// </summary>
        public bool InternalsSource { private get; set; } = false;

        public override void OnComponentAdd()
        {
            //Generate the contained atmosphere
            containedAtmopshere = DefaultGas.Generate();
            //Register atmosphere get signals
            Parent.RegisterSignal(Signal.SIGNAL_GET_ATMOSPHERE, 5, ReturnAtmosphere);
            //If we are an internal source, when equipped by a pawn
            //we want to tell the pawn this can be used as a sourceo f internals
            if (InternalsSource)
            {
                //Register equip signal
                Parent.RegisterSignal(Signal.SIGNAL_ITEM_EQUIPPED, -1, RegisterPawnInternalSource);
                Parent.RegisterSignal(Signal.SIGNAL_ITEM_UNEQUIPPED, -1, UnregisterPawnInternalSource);
            }
        }

        public override void OnComponentRemove()
        {
            //Unregister our attached signal
            Parent.UnregisterSignal(Signal.SIGNAL_GET_ATMOSPHERE, ReturnAtmosphere);
            //Unregister from parent
            if (InternalsSource)
            {
                //Register equip signal
                Parent.UnregisterSignal(Signal.SIGNAL_ITEM_EQUIPPED, RegisterPawnInternalSource);
                Parent.UnregisterSignal(Signal.SIGNAL_ITEM_UNEQUIPPED, UnregisterPawnInternalSource);
            }
        }

        /// <summary>
        /// Returns the atmosphere contained within this component.
        /// </summary>
        private Atmosphere ReturnAtmosphere(object source, params object[] data) => containedAtmopshere;

        /// <summary>
        /// Called when a pawn equips the parent of this component.
        /// The parent of this component will now be returned when the SIGNAL_PAWN_GET_INTERNAL_SOURCE
        /// signal is sent to the pawn that has the parent item equipped.
        /// </summary>
        private object RegisterPawnInternalSource(object source, params object[] data)
        {
            Pawn pawn = (Pawn)source;
            pawn.RegisterSignal(Signal.SIGNAL_PAWN_GET_INTERNAL_SOURCE, 1, ReturnParent);
            return null;
        }

        /// <summary>
        /// Called when a pawn unequips the parent of this component.
        /// </summary>
        private object UnregisterPawnInternalSource(object source, params object[] data)
        {
            Pawn pawn = (Pawn)source;
            pawn.UnregisterSignal(Signal.SIGNAL_PAWN_GET_INTERNAL_SOURCE, ReturnParent);
            return null;
        }

        /// <summary>
        /// Returns the parent of this component
        /// </summary>
        private object ReturnParent(object source, params object[] data) => Parent;

        public override void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "DefaultGas":
                    DefaultGas = (AtmosphereCreator)property;
                    return;
                case "InternalsSource":
                    InternalsSource = (bool)property;
                    return;
            }
            throw new NotImplementedException($"{name} is not a known property");
        }
    }
}
