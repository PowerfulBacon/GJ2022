using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.ComponentInterfaces.MouseEvents;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Debug
{

    /// <summary>
    /// Debug entity, add whatever you want to this for testing.
    /// </summary>
    public class DebugEntity : Entity, IMouseEnter, IMouseExit, IDestroyable
    {

        public float WorldX => Position[0] - 0.5f;

        public float WorldY => Position[1] - 0.5f;

        public float Width => 1.0f;

        public float Height => 1.0f;

        public bool Destroyed => false;

        public override Renderable Renderable { get; set; } = new StandardRenderable("error");

        public CursorSpace PositionSpace => CursorSpace.WORLD_SPACE;

        public DebugEntity(Vector<float> position) : base(position, Layers.LAYER_TURF)
        {
            MouseCollisionSubsystem.Singleton.StartTracking(this);
        }

        public void OnMouseEnter()
        {
            Log.WriteLine("Mouse entered!");
        }

        public void OnMouseExit()
        {
            Log.WriteLine("Mouse exitted!");
        }

        public override bool Destroy()
        {
            return false;
        }

    }
}
