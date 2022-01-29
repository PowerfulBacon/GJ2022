using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using System;

namespace GJ2022.Rendering.RenderSystems.Renderables
{
    class BackgroundRenderable : Renderable, IBackgroundRenderable
    {
        public override MoveDelegate moveHandler => throw new NotImplementedException();

        public override LayerChangeDelegate layerChangeHandler => throw new NotImplementedException();

        public override TextureChangeDelegate textureChangeHandler => throw new NotImplementedException();

        public override RendererTextureData GetRendererTextureData()
        {
            return TextureCache.GetTexture("error");
        }

        //===================
        // Rendering
        //===================

        //Are we rendering
        public bool IsRendering { get; private set; } = false;

        public RenderSystem<IBackgroundRenderable, BackgroundRenderSystem> RenderSystem => BackgroundRenderSystem.Singleton;

        private bool shouldContinueRendering = false;

        public override void StopRendering()
        {
            if (!IsRendering)
                return;
            shouldContinueRendering = false;
            IsRendering = false;
            RenderSystem.StopRendering(this);
            StopRenderingOverlays();
        }

        public override void StartRendering()
        {
            if (IsRendering)
                return;
            shouldContinueRendering = true;
            IsRendering = true;
            RenderSystem.StartRendering(this);
            StartRenderingOverlays();
        }

        public override void ContinueRendering()
        {
            if (!shouldContinueRendering || IsRendering)
                return;
            IsRendering = true;
            RenderSystem.StartRendering(this);
            StartRenderingOverlays();
        }

        public override void PauseRendering()
        {
            if (!IsRendering)
                return;
            IsRendering = false;
            RenderSystem.StopRendering(this);
            StopRenderingOverlays();
        }

    }
}
