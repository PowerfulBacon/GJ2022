using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Rendering;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities
{
    public abstract class Entity
    {

        //The position of the object in 3D space
        private Vector<float> _position = new Vector<float>(0, 0, 0);
        public Vector<float> Position
        {
            get { return _position; }
            set {
                IMovable movable = this as IMovable;
                if (movable == null)
                    throw new System.Exception("An entity moved but doesn't have the IMovable interface!");
                var oldPos = _position;
                _position = value;
                movable.UpdatePositionBatch();
                movable.OnMoved(oldPos);
            }
        }

        public Entity(Vector<float> position)
        {
            Position = position;
        }

    }
}
