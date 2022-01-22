using GJ2022.Rendering.Models;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Rendering.RenderSystems.Interfaces
{
    public interface IStandardRenderable : IInstanceRenderable<IStandardRenderable, InstanceRenderSystem>
    {

        Model GetModel();

        uint GetTextureUint();

        Vector GetPosition();

    }
}
