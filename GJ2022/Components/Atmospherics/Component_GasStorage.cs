using GJ2022.Atmospherics;
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

        public override void OnComponentAdd()
        {
            //Generate the contained atmosphere
            containedAtmopshere = DefaultGas.Generate();
            //Register atmosphere get signals
            Parent.RegisterSignal(Signal.SIGNAL_GET_ATMOSPHERE, 5, ReturnAtmosphere);
        }

        public override void OnComponentRemove()
        {
            //Unregister our attached signal
            Parent.UnregisterSignal(Signal.SIGNAL_GET_ATMOSPHERE, ReturnAtmosphere);
        }

        private Atmosphere ReturnAtmosphere(object source, params object[] data) => containedAtmopshere;

    }
}
