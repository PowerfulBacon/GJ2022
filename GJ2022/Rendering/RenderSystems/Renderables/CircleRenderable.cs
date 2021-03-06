using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using System;
using System.Linq;

namespace GJ2022.Rendering.RenderSystems.Renderables
{
    public class CircleRenderable : Renderable, ICircleRenderable
    {

        //The render system we are attached to
        public RenderSystem<ICircleRenderable, CircleRenderSystem> RenderSystem => CircleRenderSystem.Singleton;

        //Are we rendering
        public bool IsRendering { get; private set; } = false;
        private bool shouldContinueRendering = false;

        public override MoveDelegate moveHandler => SetPosition;
        public override LayerChangeDelegate layerChangeHandler => SetLayer;
        public override TextureChangeDelegate textureChangeHandler => throw new NotImplementedException();

        public CircleRenderable(Colour colour)
        {
            _colour = colour;
            StartRendering();
        }

        private Colour _colour;
        public Colour Colour
        {
            get { return _colour; }
            set
            {
                _colour = value;
                if (renderableBatchIndex.Count > 0)
                    (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<ICircleRenderable, CircleRenderSystem>)?.UpdateBatchData(this, 1);
            }
        }

        public override RendererTextureData GetRendererTextureData()
        {
            return TextureCache.GetTexture("error");
        }

        //===================
        // Position Handling
        //===================

        //Position value, with z-axis representing the layer
        private Vector<float> _position = new Vector<float>(0, 0, 0);

        //Get position including the layer
        public Vector<float> GetPosition()
        {
            return _position;
        }

        /// <summary>
        /// Set the position of this renderable object.
        /// Does not affect the layer.
        /// </summary>
        private void SetPosition(Vector<float> position)
        {
            _position.X = position.X;
            _position.Y = position.Y;
            //Update position in renderer
            if (renderableBatchIndex.Count > 0)
                (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<ICircleRenderable, CircleRenderSystem>)?.UpdateBatchData(this, 0);
        }

        /// <summary>
        /// Set the layer of this renderable
        /// </summary>
        private void SetLayer(float layer)
        {
            _position.Z = layer;
            //Update position in renderer
            if (renderableBatchIndex.Count > 0)
                (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<ICircleRenderable, CircleRenderSystem>)?.UpdateBatchData(this, 0);
        }

        //===================
        // Rendering
        //===================

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
