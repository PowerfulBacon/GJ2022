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
        public RenderBatchGroup(Model model, uint textureUint)
        {
            Model = model;
            TextureUint = textureUint;
        }

        public Model Model { get; }

        public uint TextureUint { get; }

    }
}
