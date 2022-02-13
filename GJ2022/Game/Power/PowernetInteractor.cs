using GJ2022.Entities;
using GJ2022.Entities.ComponentInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Game.Power
{
    /// <summary>
    /// Class you attach to machines that interact with a powernet
    /// </summary>
    public class PowernetInteractor
    {

        public Powernet _pn;
        public Powernet AttachedPowernet
        {
            get => _pn;
            set {
                //Remove our stuff from the powernet
                if (_pn != null)
                {
                    _pn.AdjustDemand(-Demand);
                    _pn.AdjustSupply(-Supply);
                }
                _pn = value;
                //Add our data to the powernet
                if (_pn != null)
                {
                    _pn.AdjustDemand(Demand);
                    _pn.AdjustSupply(Supply);
                }
            }
        }

        private float _demand;
        public float Demand
        {
            get => _demand;
            set
            {
                if (value == _demand)
                    return;
                float delta = _demand - value;
                _demand = value;
                if (_pn != null)
                    _pn.AdjustDemand(delta);
            }
        }

        private float _supply;
        public float Supply
        {
            get => _supply;
            set
            {
                if (value == _supply)
                    return;
                float delta = _supply - value;
                _supply = value;
                if (_pn != null)
                    _pn.AdjustSupply(delta);
            }
        }

    }
}
