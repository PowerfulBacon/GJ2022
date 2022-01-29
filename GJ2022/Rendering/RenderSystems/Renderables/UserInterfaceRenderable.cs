using GJ2022.Game.GameWorld;
using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.UserInterface.Components;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Rendering.RenderSystems.Renderables
{
    public class UserInterfaceRenderable : Renderable, IUserInterfaceRenderable
    {

        public override MoveDelegate moveHandler => SetPosition;

        public override LayerChangeDelegate layerChangeHandler => throw new NotImplementedException();

        public override TextureChangeDelegate textureChangeHandler => throw new NotImplementedException();

        private string icon;
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
                    (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IUserInterfaceRenderable, UserInterfaceRenderSystem>)?.UpdateBatchData(this, 2);
            }
        }

        public float Layer { get; }

        public UserInterfaceComponent.PositionModes PositionMode { get; }

        public RenderSystem<IUserInterfaceRenderable, UserInterfaceRenderSystem> RenderSystem => UserInterfaceRenderSystem.Singleton;

        private bool isRendering = false;

        public UserInterfaceRenderable(Vector<float> position, UserInterfaceComponent.PositionModes positionMode, Vector<float> scale, string icon, int layer = Layers.LAYER_USER_INTERFACE)
        {
            Position = position;
            Scale = scale;
            PositionMode = positionMode;
            this.icon = icon;
            Layer = layer;
            StartRendering();
        }

        public override RendererTextureData GetRendererTextureData()
        {
            return TextureCache.GetTexture(icon);
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
                (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IUserInterfaceRenderable, UserInterfaceRenderSystem>)?.UpdateBatchData(this, 0);
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
