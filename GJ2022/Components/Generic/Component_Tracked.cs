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

        //TODO: Handle location changing.
        public override void OnComponentAdd()
        {
            Entity parent = Parent as Entity;
            //Start tracking
            trackedX = (int)parent.Position[0];
            trackedY = (int)parent.Position[1];
            World.AddThing(Key, trackedX, trackedY, Parent);
            Parent.RegisterSignal(Signal.SIGNAL_ENTITY_MOVED, -1, OnParentMoved);
        }

        public override void OnComponentRemove()
        {
            //Stop tracking
            World.RemoveThing(Key, trackedX, trackedY, Parent);
            Parent.UnregisterSignal(Signal.SIGNAL_ENTITY_MOVED, OnParentMoved);
        }

        private object OnParentMoved(object source, params object[] parameters)
        {
            //Remove the old track
            World.RemoveThing(Key, trackedX, trackedY, Parent);
            //Establish a new track
            Entity parent = Parent as Entity;
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
            throw new NotImplementedException();
        }
    }
}
