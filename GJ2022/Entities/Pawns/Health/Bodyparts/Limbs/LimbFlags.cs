namespace GJ2022.Entities.Pawns.Health.Bodyparts.Limbs
{
    public enum LimbFlags
    {
        NONE = 0,
        //If destroyed or removed, the pawn will be killed immediately
        CRITICAL_LIMB = 1 << 0,
        //The limb cannot be removed from a pawn. If it reaches 0 health, it will be destroyed but stay in the body
        NO_REMOVAL = 1 << 1,
        //The limb cannot be inserted into a pawn. If removed, it is useless.
        NO_INSERTION = 1 << 2,
        //Limb has been destroyed
        LIMB_DESTROYED = 1 << 3,
    }
}
