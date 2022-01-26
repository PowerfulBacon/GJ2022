using GJ2022.Atmospherics.Gasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Atmospherics
{
    public struct PressurisedGas
    {

        //The actual gas being stored
        public Gas gas;
        //The amount of moles being stored
        public float moles;

        public PressurisedGas(Gas gas, float moles)
        {
            this.gas = gas;
            this.moles = moles;
        }
    }
}
