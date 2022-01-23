﻿using GJ2022.Rendering;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities
{
    public abstract class Entity
    {

        //The position of the object in 3D space
        public Vector position = new Vector(0, 0, 0);

        public Entity(Vector position)
        {
            this.position = position;
        }

    }
}
