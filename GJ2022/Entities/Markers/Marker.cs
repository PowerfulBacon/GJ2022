﻿using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Markers
{
    public abstract class Marker : Entity, IDestroyable
    {

        public bool Destroyed { get; private set; } = false;

        public Marker(Vector<float> position, float layer) : base(position, layer)
        {
            //Instantly destroy
            if (!IsValidPosition())
                Destroy();
            Marker marker = World.GetMarker((int)position[0], (int)position[1]);
            if (marker != null)
                marker.Destroy();
            World.SetMarker((int)position[0], (int)position[1], this);
        }

        public override bool Destroy()
        {
            Destroyed = true;
            World.SetMarker((int)Position[0], (int)Position[1], null);
            return base.Destroy();
        }

        public abstract bool IsValidPosition();

    }
}
