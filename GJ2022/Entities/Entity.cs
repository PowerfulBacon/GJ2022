using GJ2022.Components;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.EntityLoading;
using GJ2022.Game.GameWorld;
using GJ2022.Managers;
using GJ2022.Managers.TaskManager;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Rendering.Text;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;

namespace GJ2022.Entities
{
    public class Entity : ComponentHandler
    {

        //The renderable attached to this entity
        public virtual Renderable Renderable { get; set; }

        //Name
        public virtual string Name { get; set; }

        //Description
        public string Description { get; set; }

        //The position of the object in 2D space
        private Vector<float> _position = new Vector<float>(0, 0);

        //The location of the entity, if it is inside of something
        private Entity _location = null;

        //Contents
        public List<Entity> Contents { get; private set; } = null;

        //Texture change handler
        public string Texture { set => Renderable?.textureChangeHandler?.Invoke(value); }

        //Direction
        private Directions _direction;
        public Directions Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                Renderable?.UpdateDirection(value);
            }
        }

        //Don't set this outside of thread safe claim manager
        private bool isClaimed = false;
        public bool IsClaimed
        {
            get => isClaimed;
            set
            {
                if (value && isClaimed)
                    throw new Exception("A claim was applied on an object already claimed!");
                isClaimed = value;
            }
        }

        //The text object attached to this
        public TextObject attachedTextObject;
        public Vector<float> textObjectOffset = new Vector<float>(0, 0);

        public Entity()
        {
            
        }

        [Obsolete]
        public Entity(Vector<float> position, float layerDepreciated)
        {
            if (position.Dimensions != 2)
            {
                throw new ArgumentException($"Position provided was {position}, but should have 2 dimensions!");
            }
            Position = position;
            Renderable?.layerChangeHandler?.Invoke(layerDepreciated);
        }

        [Obsolete]
        public Entity(Entity location, float layerDepreciated)
        {
            Location = location;
            Renderable?.layerChangeHandler?.Invoke(layerDepreciated);
        }

        //Default destroy behaviour
        public virtual bool Destroy()
        {
            if (!(this is IDestroyable))
                throw new Exception("Non destroyable entity was destroyed!");
            //Remove from inventories
            Location?.RemoveFromContents(this);
            //Release our claims
            if (ThreadSafeClaimManager.HasClaim(this))
                ThreadSafeClaimManager.ReleaseClaimBlocking(this);
            //Send the destroy signal
            SendSignal(Signal.SIGNAL_ENTITY_DESTROYED);
            //Unregister all signals
            UnregisterAllSignals();
            //Stop rendering attached text
            if (attachedTextObject != null)
            {
                attachedTextObject.StopRendering();
                attachedTextObject = null;
            }
            //Stop rendering
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
            attachedTextObject?.StartRendering();
        }

        /// <summary>
        /// Stop rendering this entity.
        /// </summary>
        public void StopRendering()
        {
            Renderable?.StopRendering();
            attachedTextObject?.StopRendering();
        }

        //Location handler
        public Entity Location
        {
            get => _location;
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
                    //TODO: this will cause bugs due to start rather than continue
                    attachedTextObject?.StartRendering();
                }
                else
                {
                    Renderable?.PauseRendering();
                    attachedTextObject?.StopRendering();
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

        //Position handler
        public Vector<float> Position
        {
            get => _position;
            set
            {
                Vector<float> oldPosition = _position.Copy();
                Vector<float> delta = value - oldPosition;
                _position = value;
                Renderable?.UpdatePosition(_position);
                (this as IMoveBehaviour)?.OnMoved(oldPosition);
                if ((int)oldPosition[0] != (int)value[0] || (int)oldPosition[1] != (int)value[1])
                    SendSignal(Signal.SIGNAL_ENTITY_MOVED, (Vector<int>)oldPosition);
                if (attachedTextObject != null)
                    attachedTextObject.Position = value + textObjectOffset;
                //Change direction
                Direction = delta[0] > -delta[1]
                    ? delta[0] < delta[1] ? Directions.NORTH : Directions.EAST
                    : delta[0] < delta[1] ? Directions.WEST : Directions.SOUTH;
            }
        }

        public bool InReach(Entity target, float range = 0.5f)
        {
            return (target.Position - Position).Length() < range && Location == target.Location;
        }

        public override void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "Name":
                    Name = (string)property;
                    return;
                case "Description":
                    Description = (string)property;
                    return;
                case "Renderable":
                    Renderable = (Renderable)property;
                    return;
            }
            base.SetProperty(name, property);
        }

        //oh god this is so janky
        public override void PreInitialize(Vector<float> initializePosition)
        {
            Position = initializePosition;
        }

        public override void Initialize(Vector<float> initializePosition)
        {
            Position = initializePosition;
        }

    }
}
