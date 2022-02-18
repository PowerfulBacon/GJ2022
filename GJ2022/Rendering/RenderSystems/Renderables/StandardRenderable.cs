using GJ2022.Game.GameWorld;
using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using System.Linq;

namespace GJ2022.Rendering.RenderSystems.Renderables
{
    public class StandardRenderable : Renderable, IStandardRenderable
    {

        //The render system we are attached to
        public RenderSystem<IStandardRenderable, InstanceRenderSystem> RenderSystem => isTransparent ? TransparentRenderSystem.Singleton : InstanceRenderSystem.Singleton;

        //Are we rendering
        public bool IsRendering { get; private set; } = false;
        private bool shouldContinueRendering = false;

        public override MoveDelegate moveHandler => SetPosition;

        public override LayerChangeDelegate layerChangeHandler => SetLayer;

        public override TextureChangeDelegate textureChangeHandler => ChangeTexture;

        private string _texture;

        private bool isTransparent;

        public StandardRenderable()
        { }

        public StandardRenderable(string texture, bool isTransparent = false)
        {
            _texture = texture;
            this.isTransparent = isTransparent;
            StartRendering();
        }

        public override void UpdateDirection(Directions direction)
        {
            base.UpdateDirection(direction);
            lock (renderableBatchIndex)
                if (renderableBatchIndex.Count > 0)
                    lock (renderableBatchIndex.Keys.ElementAt(0))
                        (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IStandardRenderable, InstanceRenderSystem>)?.UpdateBatchData(this, 2);
        }

        //===================
        // Rotation Handling
        //===================

        public override void UpdateRotation(float rotation)
        {
            base.UpdateRotation(rotation);
            lock (renderableBatchIndex)
                if (renderableBatchIndex.Count > 0)
                    lock (renderableBatchIndex.Keys.ElementAt(0))
                        (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IStandardRenderable, InstanceRenderSystem>)?.UpdateBatchData(this, 1);
        }

        public float GetRotation()
        {
            return Rotation;
        }

        //===================
        // Model handling
        //===================
        private Model _model = QuadModelData.Singleton.model;

        public Model GetModel()
        {
            return _model;
        }

        //===================
        // Colour Handling
        //===================

        public void SetColour(Colour colour)
        {
            Colour = colour;
            //Update position in renderer
            lock (renderableBatchIndex)
                if (renderableBatchIndex.Count > 0)
                    lock (renderableBatchIndex.Keys.ElementAt(0))
                        (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IStandardRenderable, InstanceRenderSystem>)?.UpdateBatchData(this, 3);
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
            _position[0] = position[0];
            _position[1] = position[1];
            //Update position in renderer
            lock (renderableBatchIndex)
                if (renderableBatchIndex.Count > 0)
                    lock (renderableBatchIndex.Keys.ElementAt(0))
                    {
                        (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IStandardRenderable, InstanceRenderSystem>)?.UpdateBatchData(this, 0);
                        (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IStandardRenderable, InstanceRenderSystem>)?.UpdateBatchData(this, 1);
                    }
        }

        /// <summary>
        /// Set the layer of this renderable
        /// </summary>
        private void SetLayer(float layer)
        {
            _position[2] = layer;
            //Update position in renderer
            lock (renderableBatchIndex)
                if (renderableBatchIndex.Count > 0)
                    lock (renderableBatchIndex.Keys.ElementAt(0))
                    {
                        (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IStandardRenderable, InstanceRenderSystem>)?.UpdateBatchData(this, 0);
                        (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IStandardRenderable, InstanceRenderSystem>)?.UpdateBatchData(this, 1);
                    }
        }

        //===================
        // Texture Handling
        //===================

        private void ChangeTexture(string newTexture)
        {
            uint textureUint = _texture == null ? 0 : GetTextureUint();
            if (textureUint == TextureCache.GetTexture(newTexture).TextureUint)
            {
                _texture = newTexture;
                //Update position in renderer
                lock (renderableBatchIndex)
                    if (renderableBatchIndex.Count > 0)
                        lock (renderableBatchIndex.Keys.ElementAt(0))
                            (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<IStandardRenderable, InstanceRenderSystem>)?.UpdateBatchData(this, 2);
            }
            else if (IsRendering)
            {
                Log.WriteLine("Changed texture uint");
                //Restart rendering since the texture file changed
                StopRendering();
                _texture = newTexture;
                StartRendering();
            }
            else
            {
                _texture = newTexture;
            }
        }

        public override RendererTextureData GetRendererTextureData()
        {
            return TextureCache.GetTexture(_texture);
        }

        public uint GetTextureUint()
        {
            return GetRendererTextureData().TextureUint;
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
