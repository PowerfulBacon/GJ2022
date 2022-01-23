using GJ2022.Rendering.Models;

namespace GJ2022.Rendering.RenderSystems
{

    //The key of render batches
    public struct RenderBatchGroup
    {
        public RenderBatchGroup(Model model, uint textureUint)
        {
            Model = model;
            TextureUint = textureUint;
        }

        public Model Model { get; }

        public uint TextureUint { get; }

    }
}
