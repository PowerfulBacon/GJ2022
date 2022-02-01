using GJ2022.Atmospherics;

namespace GJ2022.Entities.Pawns.Health.Bodies
{
    public abstract class Body
    {

        //List of the name of the body slots this body uses.
        public abstract BodySlots[] BodySlots { get; }

        //Internal atmosphere of the lungs
        //Lungs handle moving gasses from the atmosphere into here
        public Atmosphere internalAtmosphere;

        //Heart handles moving gasses from internal atmosphere into the bloodstream
        //(Not realistic in code, but an optimisation the players won't see)
        //Blood stream carbon dioxide
        public float bloodstreamCarbonDioxideMoles;

        //Blood stream oxygen
        public float bloodstreamOxygenMoles;

    }
}
