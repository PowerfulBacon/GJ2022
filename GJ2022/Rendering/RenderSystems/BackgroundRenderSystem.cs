using GJ2022.Rendering.RenderSystems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Rendering.RenderSystems
{
    class BackgroundRenderSystem : RenderSystem<IBackgroundRenderable, BackgroundRenderSystem>
    {

        public static new BackgroundRenderSystem Singleton;

        protected override string SystemShaderName => "backgroundShader";

        protected override void SetSingleton()
        {
            Singleton = this;
        }

    }
}
