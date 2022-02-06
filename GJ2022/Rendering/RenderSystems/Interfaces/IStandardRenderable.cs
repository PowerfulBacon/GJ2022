using GJ2022.Game.GameWorld;
using GJ2022.Rendering.Models;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Rendering.RenderSystems.Interfaces
{
    public interface IStandardRenderable : IInstanceRenderable<IStandardRenderable, InstanceRenderSystem>
    {

        Model GetModel();

        uint GetTextureUint();

        Vector<float> GetPosition();

        float GetRotation();

        Directions Direction { get; }

    }
}
