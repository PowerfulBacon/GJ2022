namespace GJ2022.Entities.ComponentInterfaces
{
    public interface IInitializeBehaviour
    {

        /// <summary>
        /// Called when the thing is initialized.
        /// This is after new and will be after loading and creating them.
        /// </summary>
        void Initialize();

    }
}
