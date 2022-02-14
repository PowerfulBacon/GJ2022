using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Rendering.RenderSystems.Interfaces
{
    public interface IInstanceRenderable<TargetInterface, TargetRenderSystem> : IInternalRenderable
        where TargetInterface : IInternalRenderable
        where TargetRenderSystem : RenderSystem<TargetInterface, TargetRenderSystem>
    {

        RenderSystem<TargetInterface, TargetRenderSystem> RenderSystem { get; }

    }

    //NOTE:
    //I spent a really long time messing about with generic classes trying
    //to get this working, but the problem I have created is impossible to solve.
    //Since we don't actually care about what the associatedSet type is and only
    //use it as a key, just pass it as an object so it can be whatever we want.
    //This prevents us dealing with a mess of generic classes self looping.
    public interface IInternalRenderable
    {

        //Store the renderable batch index somewhere, so it can be fetched later
        void SetRenderableBatchIndex(object associatedSet, int index);

        void ClearRenderableBatchIndex(object associatedSet);

        //Fetch the renderable batch index
        int GetRenderableBatchIndex(object associatedSet);

        //Get the attached renderable texture data
        RendererTextureData GetRendererTextureData();

        //The colour of the renderable
        Colour Colour { get; }

    }

}
