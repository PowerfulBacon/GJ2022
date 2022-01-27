using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Rendering.Text;
using GJ2022.Utility.MathConstructs;
using System;
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

        //Contents
        public List<Entity> Contents { get; private set; } = null;

        //Texture change handler
        public string Texture { set { Renderable?.textureChangeHandler?.Invoke(value); } }

        //Don't set this outside of thread safe claim manager
        public bool IsClaimed { get; set; } = false;

        //The text object attached to this
        protected TextObject attachedTextObject;

        public Entity(Vector<float> position, float layer)
        {
            if (position.Dimensions != 2)
            {
                throw new ArgumentException($"Position provided was {position}, but should have 2 dimensions!");
            }
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
            if (!(this is IDestroyable))
                throw new System.Exception("Non destroyable entity was destroyed!");
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
                Entity oldLocation = _location;
                //Remove ourselves from the old contents
                _location?.RemoveFromContents(this);
                //Set the location
                _location = value;
                //If we changed location, pause / resume rendering.
                if (value == null)
                {
                    Renderable?.ContinueRendering();
                }
                else
                {
                    Renderable?.PauseRendering();
                }
                //Add ourselves to the new contents
                _location?.AddToContents(this);
                //Run the on move
                (this as IMoveBehaviour)?.OnMoved(oldLocation);
            }
        }

        private void AddToContents(Entity entity)
        {
            if (Contents == null)
                Contents = new List<Entity>();
            Contents.Add(entity);
        }

        private void RemoveFromContents(Entity entity)
        {
            Contents.Remove(entity);
            if (Contents.Count == 0)
                Contents = null;
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
                Vector<float> oldPosition = _position;
                _position = value;
                Renderable?.moveHandler?.Invoke(_position);
                (this as IMoveBehaviour)?.OnMoved(oldPosition);
                if(attachedTextObject != null)
                    attachedTextObject.Position = value;
            }
        }

        public bool InReach(Entity target, float range = 0.5f)
        {
            return (target.Position - Position).Length() < range && Location == target.Location;
        }

    }
}
