using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.ComponentInterfaces
{
    public interface IMoveBehaviour
    {

        void OnMoved(Vector<float> oldPosition);
        void OnMoved(Entity oldLocation);

    }
}
