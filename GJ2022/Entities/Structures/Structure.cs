﻿using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Structures
{
    public abstract class Structure : Entity
    {

        public Structure(Vector<float> position, float layer) : base(position, layer)
        {

        }

    }
}