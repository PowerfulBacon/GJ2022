namespace GJ2022.Rendering.RenderSystems
{
    //TODO
    public class OutlineQuadRenderSystem : InstanceRenderSystem
    {

        public static new OutlineQuadRenderSystem Singleton;

        protected override string SystemShaderName => "outlineShader";

        protected override void SetSingleton()
        {
            Singleton = this;
        }

    }
}
