namespace GJ2022.PawnBehaviours
{
    public enum PawnHazards
    {
        NONE = 0,
        ALL = ~0,
        //Low pressure hazard, wear a pressure-resistant suit
        HAZARD_LOW_PRESSURE = 1 << 0,
        //High pressure hazard, wear a high-pressure-resistant suit
        HAZARD_HIGH_PRESSURE = 1 << 1,
        //Breath hazard, internals required
        HAZARD_BREATH = 1 << 2,
        //Gravity hazard, jetpack required / only move in a straight line
        HAZARD_GRAVITY = 1 << 3,
    }
}
