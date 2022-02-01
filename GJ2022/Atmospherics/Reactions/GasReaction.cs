﻿using GJ2022.Atmospherics.Gasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Atmospherics.Reactions
{
    public abstract class GasReaction
    {

        public abstract Dictionary<Gas, float> RequiredReactants { get; }

        public virtual Dictionary<Gas, float> RequiredCatalysts { get; } = new Dictionary<Gas, float>();

        public virtual Dictionary<Gas, float> CreatedGasses { get; } = new Dictionary<Gas, float>();

        public virtual float MinimumTemperature { get; } = 0;
        public virtual float MaximumTemperature { get; } = float.MaxValue;

        public void Process(Atmosphere atmosphere)
        {
            if (!CheckReaction(atmosphere))
                return;
            DoReaction(atmosphere);
        }

        public virtual bool CheckReaction(Atmosphere atmosphere)
        {
            //Check temperature
            if (atmosphere.KelvinTemperature < MinimumTemperature || atmosphere.KelvinTemperature > MaximumTemperature)
                return false;
            //Check reactants
            foreach (Gas gas in RequiredReactants.Keys)
            {
                if (!atmosphere.atmosphericContents.ContainsKey(gas) || atmosphere.atmosphericContents[gas].moles < RequiredReactants[gas])
                    return false;
            }
            //Check catalysts
            foreach (Gas gas in RequiredCatalysts.Keys)
            {
                if (!atmosphere.atmosphericContents.ContainsKey(gas) || atmosphere.atmosphericContents[gas].moles < RequiredCatalysts[gas])
                    return false;
            }
            return true;
        }

        public virtual void DoReaction(Atmosphere atmosphere)
        {
            
        }

    }
}
