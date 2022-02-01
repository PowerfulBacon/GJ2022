using GJ2022.Atmospherics.Block;
using GJ2022.Atmospherics.Gasses;
using System;
using System.Collections.Generic;

namespace GJ2022.Atmospherics
{
    public class Atmosphere
    {

        public static Atmosphere SpaceAtmosphere => new Atmosphere(1);
        public static Atmosphere IdealAtmosphere => new Atmosphere(AtmosphericConstants.IDEAL_TEMPERATURE, new PressurisedGas(Nitrogen.Singleton, 82), new PressurisedGas(Oxygen.Singleton, 22));

        //Attached atmospheric block
        public AtmosphericBlock attachedBlock;

        //The atmospheric contents of this atmosphere
        public Dictionary<Gas, PressurisedGas> AtmosphericContents { get; private set; } = new Dictionary<Gas, PressurisedGas>();

        //Temperature of this atmosphere
        public float KelvinTemperature { get; private set; }

        //The volume in litres of this atmosphere
        public float LitreVolume { get; private set; }

        //Pressure in kPa
        public float KiloPascalPressure { get; private set; }

        //Moles of gas in this atmosphere
        public float Moles
        {
            get
            {
                float _moles = 0;
                foreach (PressurisedGas gas in AtmosphericContents.Values)
                {
                    _moles += gas.moles;
                }
                //Must not be 0 or a division by 0 error occurs.
                return Math.Max(_moles, 0.0001f);
            }
        }

        public Atmosphere(float temperature, params PressurisedGas[] gasses)
        {
            //Set the temperature
            KelvinTemperature = temperature;
            //Add the gases
            foreach (PressurisedGas gas in gasses)
            {
                AtmosphericContents.Add(gas.gas, gas);
            }
        }

        /// <summary>
        /// Attach this to an atmospheric block
        /// </summary>
        public void AttachBlock(AtmosphericBlock block)
        {
            attachedBlock = block;
            attachedBlock.UpdateGasTurfs();
        }

        public float GetMoles(Gas gas)
        {
            return AtmosphericContents.ContainsKey(gas) ? AtmosphericContents[gas].moles : 0;
        }

        public void SetTemperature(float newTemperature)
        {
            KelvinTemperature = newTemperature;
            KiloPascalPressure = AtmosphericConstants.CalculatePressure(LitreVolume, KelvinTemperature, Moles);
        }

        /// <summary>
        /// Adds gas, the new gas adopts the temperature of the existing gas.
        /// If recalculate is set to true, all gas turfs will be updated.
        /// </summary>
        public void SetMoles(Gas gas, float newMoles, bool recalculate = true)
        {
            if (newMoles > 0)
            {
                //Calculate constant
                //(pressure * volume = c)
                if (AtmosphericContents.ContainsKey(gas))
                {
                    PressurisedGas foundGas = AtmosphericContents[gas];
                    foundGas.moles = newMoles;
                }
                else
                    AtmosphericContents.Add(gas, new PressurisedGas(gas, newMoles));
            }
            else
            {
                if (AtmosphericContents.ContainsKey(gas))
                    AtmosphericContents.Remove(gas);
            }
            if (!recalculate)
                return;
            //Calculate pressure and temp
            KiloPascalPressure = AtmosphericConstants.CalculatePressure(LitreVolume, KelvinTemperature, Moles);
            //Send and atmospheric recalculate to the turfs
            attachedBlock?.UpdateGasTurfs();
        }

        //Set the volume of the atmosphere, recalculate pressure.
        //Assume that the temperature doesn't change when we change the room size
        //(Building a wall between 2 rooms shouldn't cause the room to go to like 500000 kelvin).
        public void SetVolume(float litres)
        {
            LitreVolume = litres;
            KiloPascalPressure = AtmosphericConstants.CalculatePressure(LitreVolume, KelvinTemperature, Moles);
            //KiloPascalPressure = AtmosphericConstants.CalculatePressure(LitreVolume);
            //KelvinTemperature = AtmosphericConstants.CalculateTemperature(KiloPascalPressure, LitreVolume, Moles);
        }

        //Merge another atmosphere into our own
        //This reuses elements from the other atmosphere, assuming it will be deleted.
        public void Merge(Atmosphere other)
        {
            //Inherit temperature (Take an average of the areas temp per tile)
            float totalMoles = other.Moles + Moles;
            float totalTemperature = (other.Moles * other.KelvinTemperature) + (KelvinTemperature * Moles);
            KelvinTemperature = totalTemperature / totalMoles;
            //Calculate new gasses
            foreach (PressurisedGas newGas in other.AtmosphericContents.Values)
            {
                if (AtmosphericContents.ContainsKey(newGas.gas))
                {
                    //Increase the amount of moles that we have
                    PressurisedGas existingGas = AtmosphericContents[newGas.gas];
                    existingGas.moles += newGas.moles;
                }
                else
                {
                    //Add the gas
                    AtmosphericContents.Add(newGas.gas, newGas);
                }
            }
            //Adjust volume: Recalculates pressure and temperature
            SetVolume(LitreVolume + other.LitreVolume);
            //Send and atmospheric recalculate to the turfs
            attachedBlock?.UpdateGasTurfs();
        }

        /// <summary>
        /// Equalize gasses with another atmosphere
        /// </summary>
        public void Equalize(Atmosphere other)
        {
            //Equalize temperature
            float totalMoles = other.Moles + Moles;
            float totalTemperature = (other.Moles * other.KelvinTemperature) + (KelvinTemperature * Moles);
            other.KelvinTemperature = totalTemperature / totalMoles;
            KelvinTemperature = totalTemperature / totalMoles;
            //Equalize gasses (Do it via moles since temperatures are the same)
            Dictionary<Gas, float> sharedMoles = new Dictionary<Gas, float>();
            //Get the gasses
            foreach (PressurisedGas gas in AtmosphericContents.Values)
            {
                sharedMoles.Add(gas.gas, gas.moles);
            }
            foreach (PressurisedGas gas in other.AtmosphericContents.Values)
            {
                if (sharedMoles.ContainsKey(gas.gas))
                    sharedMoles[gas.gas] += gas.moles;
                else
                    sharedMoles.Add(gas.gas, gas.moles);
            }
            //Move the gasses
            //Clear existing gasses
            AtmosphericContents.Clear();
            other.AtmosphericContents.Clear();
            //Calculate proportions
            float proportionThis = LitreVolume / (LitreVolume + other.LitreVolume);
            float proportionOther = other.LitreVolume / (LitreVolume + other.LitreVolume);
            //Add new gasses
            foreach (Gas gas in sharedMoles.Keys)
            {
                float moles = sharedMoles[gas];
                AtmosphericContents.Add(gas, new PressurisedGas(gas, moles * proportionThis));
                other.AtmosphericContents.Add(gas, new PressurisedGas(gas, moles * proportionOther));
            }
            //Calculate pressure
            //Calculate pressure and temp
            KiloPascalPressure = AtmosphericConstants.CalculatePressure(LitreVolume, KelvinTemperature, Moles);
            //Send and atmospheric recalculate to the turfs
            attachedBlock?.UpdateGasTurfs();
            other.attachedBlock?.UpdateGasTurfs();
        }

        public void InheritGasProportion(Atmosphere other, float proportion)
        {
            //Inherit temperature (Take an average of the areas temp per tile)
            float totalMoles = other.Moles + Moles;
            float totalTemperature = (other.Moles * other.KelvinTemperature) + (KelvinTemperature * Moles);
            KelvinTemperature = totalTemperature / totalMoles;
            //Yoink the gasses
            foreach (PressurisedGas gas in other.AtmosphericContents.Values)
            {
                if (AtmosphericContents.ContainsKey(gas.gas))
                    AtmosphericContents[gas.gas].moles += gas.moles * proportion;
                else
                    AtmosphericContents.Add(gas.gas, new PressurisedGas(gas.gas, gas.moles * proportion));
            }
            //Calculate temp
            KiloPascalPressure = AtmosphericConstants.CalculatePressure(LitreVolume, KelvinTemperature, Moles);
            //Send and atmospheric recalculate to the turfs
            attachedBlock?.UpdateGasTurfs();
        }

    }
}
