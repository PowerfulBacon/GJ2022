namespace GJ2022.Entities.Items.Clothing
{
    public enum BodyCoverFlags
    {
        NONE = 0,
        COVER_LEGS = 1 << 0,
        COVER_FEET = 1 << 1,
        COVER_ARMS = 1 << 2,
        COVER_HANDS = 1 << 3,
        COVER_BODY = 1 << 4,
        COVER_HEAD = 1 << 5,
        COVER_MOUTH = 1 << 6,
        COVER_EYES = 1 << 7,
        COVER_TAIL = 1 << 8,
    }
}
