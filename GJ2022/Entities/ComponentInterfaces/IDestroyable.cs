namespace GJ2022.Entities.ComponentInterfaces
{
    public interface IDestroyable
    {

        //Returns false if destruction was insuccessful
        bool Destroy();

        bool IsDestroyed();

    }
}
