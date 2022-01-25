namespace GJ2022.Entities.ComponentInterfaces.MouseEvents
{

    public enum CursorSpace
    {
        WORLD_SPACE,
        SCREEN_SPACE
    }

    public interface IMouseEvent
    {

        CursorSpace PositionSpace { get; }

        float WorldX { get; }

        float WorldY { get; }

        float Width { get; }

        float Height { get; }

    }
}
