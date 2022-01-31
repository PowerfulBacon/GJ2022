using GJ2022.Game.GameWorld;
using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.UserInterface.Components;
using GJ2022.Utility.MathConstructs;
using System;
using System.Linq;

namespace GJ2022.Rendering.RenderSystems.Renderables
{
    public class ButtonRenderable : Renderable, IButtonRenderable
    {

        public override MoveDelegate moveHandler => SetPosition;

        public override LayerChangeDelegate layerChangeHandler => throw new NotImplementedException();

        public override TextureChangeDelegate textureChangeHandler => throw new NotImplementedException();

        public override ButtonStateChangeDelegate buttonStateChangeHandler => SetButtonHoverState;

        public Model Model { get; }

        private Vector<float> _position = new Vector<float>(0, 0);
        public Vector<float> Position
        {
            get { return _position; }
            set
            {
                UpdatePosition(value);
            }
        }

        private Vector<float> _scale;
        public Vector<float> Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                if (renderableBatchIndex.Count > 0)
                    (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IButtonRenderable, ButtonRenderSystem>)?.UpdateBatchData(this, 1);
            }
        }

        public float Layer { get; }

        public UserInterfaceComponent.PositionModes PositionMode { get; }

        public RenderSystem<IButtonRenderable, ButtonRenderSystem> RenderSystem => ButtonRenderSystem.Singleton;

        public Colour Colour { get; set; }

        private bool _isHovered;
        public bool isHovered => _isHovered;

        RenderSystem<IButtonRenderable, ButtonRenderSystem> IInstanceRenderable<IButtonRenderable, ButtonRenderSystem>.RenderSystem => ButtonRenderSystem.Singleton;

        private bool isRendering = false;

        public ButtonRenderable(Vector<float> position, UserInterfaceComponent.PositionModes positionMode, Colour colour, Vector<float> scale, int layer = Layers.LAYER_USER_INTERFACE)
        {
            Position = position;
            Scale = scale;
            PositionMode = positionMode;
            Colour = colour;
            Layer = layer;
            StartRendering();
        }

        public override RendererTextureData GetRendererTextureData()
        {
            return TextureCache.GetTexture("error");
        }

        private void SetButtonHoverState(bool hovered)
        {
            _isHovered = hovered;
            if (renderableBatchIndex.Count > 0)
                (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IButtonRenderable, ButtonRenderSystem>)?.UpdateBatchData(this, 2);
        }

        /// <summary>
        /// Set the position of this renderable object.
        /// Does not affect the layer.
        /// </summary>
        private void SetPosition(Vector<float> position)
        {
            _position[0] = position[0];
            _position[1] = position[1];
            //Update position in renderer
            if (renderableBatchIndex.Count > 0)
                (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IButtonRenderable, ButtonRenderSystem>)?.UpdateBatchData(this, 0);
        }

        public override void StartRendering()
        {
            if (isRendering)
                return;
            isRendering = true;
            RenderSystem.StartRendering(this);
            StartRenderingOverlays();
        }

        public override void StopRendering()
        {
            if (!isRendering)
                return;
            isRendering = false;
            RenderSystem.StopRendering(this);
            StopRenderingOverlays();
        }

        public override void PauseRendering()
        {
            throw new NotImplementedException();
        }

        public override void ContinueRendering()
        {
            throw new NotImplementedException();
        }

    }
}
