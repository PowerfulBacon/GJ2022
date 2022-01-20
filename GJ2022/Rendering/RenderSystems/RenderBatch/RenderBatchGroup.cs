using GJ2022.Rendering.Models;
using GJ2022.Rendering.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Rendering.RenderSystems
{

    //The key of render batches
    public struct RenderBatchGroup
    {
        public RenderBatchGroup(ShaderSet shaders, Model model, uint textureUint)
        {
            Shaders = shaders;
            Model = model;
            TextureUint = textureUint;
        }

        public ShaderSet Shaders { get; }

        public Model Model { get; }

        public uint TextureUint { get; }

    }
}
