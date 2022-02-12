namespace GJ2022.Entities.Pawns.Health
{
    public enum BodySlots
    {
        //The body - main part for all mobs
        //Body cannot be removed and replaced
        SLOT_BODY,
        //Head also cannot be removed as mobs die without it
        SLOT_HEAD,

        //Humanoids
        SLOT_LEG_LEFT,
        SLOT_LEG_RIGHT,
        SLOT_ARM_LEFT,
        SLOT_ARM_RIGHT,
        SLOT_TAIL,          //oh god oh god why are these part of humanoids :fearful:

        //Generic Animals
        SLOT_LEG_FRONT_LEFT,
        SLOT_LEG_FRONT_RIGHT,
        SLOT_LEG_BACK_LEFT,
        SLOT_LEG_BACK_RIGHT,

    }
}
