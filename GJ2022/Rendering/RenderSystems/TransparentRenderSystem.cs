namespace GJ2022.Rendering.RenderSystems
{
    public class TransparentRenderSystem : InstanceRenderSystem
    {

        public static new InstanceRenderSystem Singleton;

        protected override void SetSingleton()
        {
            Singleton = this;
        }

    }
}
