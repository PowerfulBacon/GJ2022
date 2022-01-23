using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Blueprints
{
    public class TurfBlueprint : Blueprint
    {

        public TurfBlueprint(Vector position, string texture, Type createdType, int priority) : base(position, texture, createdType, priority)
        { }

        public override void Complete()
        {
            //Destroy existing one
            World.SetTurf((int)position[0], (int)position[1], null);
            //Create an instance of the thingy
            Activator.CreateInstance(CreatedType, (int)position[0], (int)position[1]);
            //Destroy the blueprint
            Destroy();
        }

    }
}
