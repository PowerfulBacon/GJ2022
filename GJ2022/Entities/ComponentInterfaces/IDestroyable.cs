namespace GJ2022.Entities.ComponentInterfaces
{
    public interface IDestroyable
    {

        bool Destroyed { get; }

        //Returns false if destruction was insuccessful
        bool Destroy();

    }
}
