using GJ2022.Utility.MathConstructs;

namespace GJ2022.Rendering.RenderSystems.Interfaces
{
    public interface ICircleRenderable : IInstanceRenderable<ICircleRenderable, CircleRenderSystem>
    {

        Colour Colour { get; }

        Vector<float> GetPosition();

    }
}
