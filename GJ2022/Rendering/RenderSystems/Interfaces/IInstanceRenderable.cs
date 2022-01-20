using GJ2022.Utility.MathConstructs;

namespace GJ2022.Rendering.RenderSystems.Interfaces
{
    public interface IInstanceRenderable
    {

        void SetRenderableBatchIndex(RenderBatchSet associatedSet, int index);

        int GetRenderableBatchIndex(RenderBatchSet associatedSet);

        void UpdateBatchLighting();

        //Required getters
        Vector GetInstancePosition();

    }
}
