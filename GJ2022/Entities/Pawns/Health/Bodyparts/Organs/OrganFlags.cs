namespace GJ2022.Entities.Pawns.Health.Bodyparts.Organs
{
    public enum OrganFlags
    {
        NONE = 0,
        //Organ is destroyed
        ORGAN_DESTROYED = 1 << 0,
        //Organ is failing
        ORGAN_FAILING = 1 << 1,
        //Organ performs processing
        ORGAN_PROCESSING = 1 << 2,
    }
}
