using GJ2022.Entities;
using GJ2022.Game.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Generic
{

    public class Component_Tracked : Component
    {

        public string Key { get; set; }

        private int trackedX;
        private int trackedY;

        public override void OnComponentAdd()
        {
            Entity parent = Parent as Entity;
            if (parent.Location != null)
                return;
            //Start tracking
            trackedX = (int)parent.Position[0];
            trackedY = (int)parent.Position[1];
            World.AddThing(Key, trackedX, trackedY, Parent);
            Parent.RegisterSignal(Signal.SIGNAL_ENTITY_MOVED, -1, OnParentMoved);
            Parent.RegisterSignal(Signal.SIGNAL_ENTITY_LOCATION, -1, OnLocationChanged);
        }

        public override void OnComponentRemove()
        {
            //Stop tracking
            Entity parent = Parent as Entity;
            //No track will exist
            if (parent.Location == null)
                World.RemoveThing(Key, trackedX, trackedY, Parent);
            Parent.UnregisterSignal(Signal.SIGNAL_ENTITY_MOVED, OnParentMoved);
            Parent.UnregisterSignal(Signal.SIGNAL_ENTITY_LOCATION, OnLocationChanged);
        }

        private object OnLocationChanged(object source, params object[] parameters)
        {
            Entity oldLocation = parameters[0] as Entity;
            Entity newLocation = parameters[1] as Entity;
            
            //Remove the old track
            if(newLocation != null && oldLocation == null)
                World.RemoveThing(Key, trackedX, trackedY, Parent);
            //Start tracking
            if (newLocation == null && oldLocation != null)
            {
                Entity parent = Parent as Entity;
                trackedX = (int)parent.Position[0];
                trackedY = (int)parent.Position[1];
                World.AddThing(Key, trackedX, trackedY, Parent);
            }
            return null;
        }

        private object OnParentMoved(object source, params object[] parameters)
        {
            //Establish a new track
            Entity parent = Parent as Entity;
            //No track will exist
            if (parent.Location != null)
                return null;
            //Remove the old track
            World.RemoveThing(Key, trackedX, trackedY, Parent);
            //Start tracking
            trackedX = (int)parent.Position[0];
            trackedY = (int)parent.Position[1];
            World.AddThing(Key, trackedX, trackedY, Parent);

            return null;
        }

        public override void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "Key":
                    Key = (string)property;
                    return;
            }
            throw new NotImplementedException($"Invalid property {name}");
        }
    }
}
