namespace GJ2022.Rendering.Models
{
    public enum CubeFaceFlags
    {
        FACE_NONE = 0,
        FACE_ALL = ~0,
        FACE_ABOVE = 1,
        FACE_BELOW = 2,
        FACE_FRONT = 4,
        FACE_BACK = 8,
        FACE_RIGHT = 16,
        FACE_LEFT = 32
    }

}
