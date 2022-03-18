using GJ2022.Entities;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Areas
{
    public class Area : Entity, IDestroyable
    {

        public bool Destroyed { get; private set; } = false;

        public Area() : base()
        { }

        public override bool Destroy()
        {
            Destroyed = true;
            World.Current.SetArea((int)Position.X, (int)Position.Y, null);
            return base.Destroy();
        }

        public override void Initialize(Vector<float> initializePosition)
        {
            base.Initialize(initializePosition);
            World.Current.GetArea((int)initializePosition.X, (int)initializePosition.Y)?.Destroy();
            World.Current.SetArea((int)initializePosition.X, (int)initializePosition.Y, this);
        }
    }
}
