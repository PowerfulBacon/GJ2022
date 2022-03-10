using GJ2022.EntityLoading;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;

namespace GJ2022.Rendering.RenderSystems.Renderables
{
    public abstract class Renderable : IInternalRenderable, IInstantiatable
    {

        //Delegates
        public delegate void MoveDelegate(Vector<float> position);
        public delegate void LayerChangeDelegate(float layer);
        public delegate void TextureChangeDelegate(string texture);
        public delegate void ButtonStateChangeDelegate(bool hovered);

        //Events
        public abstract MoveDelegate moveHandler { get; }
        public abstract LayerChangeDelegate layerChangeHandler { get; }
        public abstract TextureChangeDelegate textureChangeHandler { get; }
        public virtual ButtonStateChangeDelegate buttonStateChangeHandler { get; }

        public abstract RendererTextureData GetRendererTextureData();

        protected Dictionary<object, int> renderableBatchIndex = new Dictionary<object, int>();

        public abstract void StopRendering();
        public abstract void StartRendering();
        public abstract void ContinueRendering();
        public abstract void PauseRendering();

        public float Rotation { get; private set; } = 0;

        public virtual void UpdateRotation(float rotation)
        {
            Rotation = rotation;
            if (Overlays == null)
                return;
            lock (Overlays)
            {
                foreach (Renderable overlay in Overlays.Values)
                {
                    overlay.UpdateRotation(rotation);
                }
            }
        }

        public Directions Direction { get; private set; } = Directions.NONE;

        public virtual void UpdateDirection(Directions direction)
        {
            Direction = direction;
            if (Overlays == null)
                return;
            lock (Overlays)
            {
                foreach (Renderable overlay in Overlays.Values)
                {
                    overlay.UpdateDirection(direction);
                }
            }
        }

        public void UpdatePosition(Vector<float> position)
        {
            moveHandler?.Invoke(position);
            _overlayPosition = position;
            UpdateOvelayPosition(position);
        }

        public void SetRenderableBatchIndex(object associatedSet, int index)
        {
            lock (renderableBatchIndex)
            {
                if (renderableBatchIndex.ContainsKey(associatedSet))
                    renderableBatchIndex[associatedSet] = index;
                else
                    renderableBatchIndex.Add(associatedSet, index);
            }
        }

        /// <summary>
        /// Returns the renderable batch index in the provided set.
        /// Returns -1 if failed.
        /// </summary>
        public int GetRenderableBatchIndex(object associatedSet)
        {
            lock (renderableBatchIndex)
            {
                if (renderableBatchIndex.ContainsKey(associatedSet))
                    return renderableBatchIndex[associatedSet];
                else
                    return -1;
            }
        }

        public void ClearRenderableBatchIndex(object associatedSet)
        {
            lock (renderableBatchIndex)
            {
                if (renderableBatchIndex.ContainsKey(associatedSet))
                    renderableBatchIndex.Remove(associatedSet);
            }
        }

        //Overlays
        private Dictionary<string, Renderable> Overlays { get; set; } = null;

        //Colour
        public Colour Colour { get; protected set; } = Colour.White;

        private Vector<float> _overlayPosition = new Vector<float>(0, 0);

        private void UpdateOvelayPosition(Vector<float> position)
        {
            if (Overlays == null)
                return;
            lock (Overlays)
            {
                foreach (Renderable overlay in Overlays.Values)
                {
                    overlay.UpdatePosition(position);
                }
            }
        }

        public void StopRenderingOverlays()
        {
            if (Overlays == null)
                return;
            lock (Overlays)
            {
                foreach (Renderable overlay in Overlays.Values)
                {
                    overlay.StopRendering();
                }
            }
        }

        public void StartRenderingOverlays()
        {
            if (Overlays == null)
                return;
            lock (Overlays)
            {
                foreach (Renderable overlay in Overlays.Values)
                {
                    overlay.StartRendering();
                }
            }
        }

        public bool HasOverlay(string id)
        {
            return Overlays?.ContainsKey(id) ?? false;
        }

        public void AddOverlay(string id, Renderable overlay, float layer)
        {
            if (Overlays == null)
                Overlays = new Dictionary<string, Renderable>();
            overlay.UpdatePosition(_overlayPosition);
            overlay.layerChangeHandler?.Invoke(layer);
            overlay.UpdateDirection(Direction);
            overlay.UpdateRotation(Rotation);
            lock (Overlays)
            {
                Overlays.Add(id, overlay);
            }
        }

        public void ClearOverlays()
        {
            if (Overlays == null)
                return;
            lock (Overlays)
            {
                foreach (Renderable overlay in Overlays.Values)
                {
                    overlay.StopRendering();
                }
                Overlays.Clear();
            }
        }

        public void RemoveOvelay(string id)
        {
            lock (Overlays)
            {
                if (Overlays.ContainsKey(id))
                {
                    Overlays[id].StopRendering();
                    Overlays.Remove(id);
                }
            }
        }

        public virtual void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "Texture":
                    textureChangeHandler.Invoke((string)property);
                    return;
                case "Rotation":
                    UpdateRotation(Convert.ToSingle(property));
                    return;
                case "Layer":
                    layerChangeHandler.Invoke(Convert.ToSingle(property));
                    return;
                case "Overlays":
                    //TODO
                    break;
            }
            throw new System.NotImplementedException($"Renderable doesn't have a handler for the property {name}.");
        }

        public void Initialize(Vector<float> initializePosition)
        {
            StartRendering();
        }

        public void PreInitialize(Vector<float> initializePosition)
        { }
    }
}
