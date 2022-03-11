﻿using GJ2022.Game.Construction.Blueprints;
using GJ2022.Utility.MathConstructs;
using System;

namespace GJ2022.Entities.Blueprints
{
    public class TurfBlueprint : Blueprint
    {

        public TurfBlueprint(Vector<float> position, BlueprintDetail detail) : base(position, detail)
        { }

        public override void Complete()
        {
            //Create an instance of the thingy
            Activator.CreateInstance(BlueprintDetail.CreatedType, (int)Position.X, (int)Position.Y);
            //Destroy the blueprint
            Destroy();
        }

    }
}
