using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Entities.Items;
using GJ2022.Game.Construction.Blueprints;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Blueprints
{
    public class BlueprintDirectional : Blueprint
    {

        private Directions direction = Directions.NORTH;

        public BlueprintDirectional(Vector<float> position, BlueprintDetail blueprint) : base(position, blueprint)
        {
        }

        public override void Complete()
        {
            //Clear and delete all contents
            foreach (Item item in contents)
            {
                item.Destroy();
            }
            contents.Clear();
            //Create an instance of the thingy
            Activator.CreateInstance(BlueprintDetail.CreatedType, Position, direction);
            //Destroy the blueprint
            Destroy();
        }
    }
}
