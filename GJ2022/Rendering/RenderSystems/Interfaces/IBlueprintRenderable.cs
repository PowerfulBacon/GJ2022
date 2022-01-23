using GJ2022.Rendering.Models;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Rendering.RenderSystems.Interfaces
{
    public interface IBlueprintRenderable : IInstanceRenderable<IBlueprintRenderable, BlueprintRenderSystem>
    {

        Model GetModel();

        uint GetTextureUint();

        Vector<float> GetPosition();

    }
}
