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
            World.SetArea((int)Position[0], (int)Position[1], null);
            return base.Destroy();
        }

        public override void Initialize(Vector<float> initializePosition)
        {
            base.Initialize(initializePosition);
            World.GetArea((int)initializePosition[0], (int)initializePosition[1])?.Destroy();
            World.SetArea((int)initializePosition[0], (int)initializePosition[1], this);
        }
    }
}
