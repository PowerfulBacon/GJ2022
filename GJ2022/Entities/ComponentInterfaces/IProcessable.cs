namespace GJ2022.Entities.ComponentInterfaces
{
    public interface IProcessable : IDestroyable
    {

        void Process(float deltaTime);

    }
}
