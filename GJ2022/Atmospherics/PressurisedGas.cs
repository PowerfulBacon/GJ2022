using GJ2022.Atmospherics.Gasses;

namespace GJ2022.Atmospherics
{
    public class PressurisedGas
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
