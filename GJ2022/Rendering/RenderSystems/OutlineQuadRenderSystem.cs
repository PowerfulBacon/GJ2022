using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Rendering.RenderSystems
{
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
