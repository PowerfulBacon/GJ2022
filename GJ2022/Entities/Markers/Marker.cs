using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Markers
{
    public abstract class Marker : Entity, IDestroyable
    {

        public bool Destroyed { get; private set; } = false;

        public Marker(Vector<float> position, float layer) : base(position, layer)
        {
            //Instantly destroy
            if (!IsValidPosition())
            {
                Destroy();
                return;
            }
            Marker marker = World.Current.GetMarker((int)position.X, (int)position.Y);
            if (marker != null)
            {
                marker.Destroy();
                return;
            }
            World.Current.SetMarker((int)position.X, (int)position.Y, this);
        }

        public override bool Destroy()
        {
            Destroyed = true;
            World.Current.SetMarker((int)Position.X, (int)Position.Y, null);
            return base.Destroy();
        }

        public abstract bool IsValidPosition();

    }
}
