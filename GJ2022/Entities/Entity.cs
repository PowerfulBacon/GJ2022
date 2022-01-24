using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.Entities
{
    public abstract class Entity
    {

        //The renderable attached to this entity
        protected abstract Renderable Renderable { get; set; }

        //The layer of the object
        private float _layer = 0;

        //The position of the object in 2D space
        private Vector<float> _position = new Vector<float>(0, 0);

        //The location of the entity, if it is inside of something
        private Entity _location = null;

        //Lazylist of the contents of this entity, if we have any
        private List<Entity> _contents = null;

        //Texture change handler
        public string Texture { set { Renderable?.textureChangeHandler?.Invoke(value); } }

        public Entity(Vector<float> position, float layer)
        {
            Position = position;
            Layer = layer;
        }

        public Entity(Entity location, float layer)
        {
            Location = location;
            Layer = layer;
        }

        //Default destroy behaviour
        public virtual bool Destroy()
        {
            Log.WriteLine("banana");
            Renderable?.StopRendering();
            Renderable = null;
            return true;
        }

        /// <summary>
        /// Start rendering this entity
        /// </summary>
        public void StartRendering()
        {
            Renderable?.StartRendering();
        }

        /// <summary>
        /// Stop rendering this entity.
        /// </summary>
        public void StopRendering()
        {
            Renderable?.StopRendering();
        }

        //Location handler
        public Entity Location
        {
            get { return _location; }
            set
            {
                //Remove ourselves from the old contents
                _location?.RemoveFromContents(this);
                //If we changed location, pause / resume rendering.
                if (value == null)
                {
                    Renderable?.ContinueRendering();
                }
                else
                {
                    Renderable?.PauseRendering();
                }
                //Set the location
                _location = value;
                //Add ourselves to the new contents
                _location?.AddToContents(this);
            }
        }

        private void AddToContents(Entity entity)
        {
            if (_contents == null)
                _contents = new List<Entity>();
            _contents.Add(entity);
        }

        private void RemoveFromContents(Entity entity)
        {
            _contents.Remove(entity);
            if (_contents.Count == 0)
                _contents = null;
        }

        //Layer handler
        public float Layer
        {
            get { return _layer; }
            set
            {
                _layer = value;
                Renderable?.layerChangeHandler?.Invoke(_layer);
            }
        }

        //Position handler
        public Vector<float> Position
        {
            get { return _position; }
            set
            {
                _position = value;
                Renderable?.moveHandler?.Invoke(_position);
            }
        }

    }
}
