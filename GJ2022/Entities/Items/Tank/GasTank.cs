using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Atmospherics;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.Tank
{
    public abstract class GasTank : Item
    {

        //Contained atmosphere of this gas tank
        public abstract Atmosphere ContainedAtmosphere { get; }

        public GasTank(Vector<float> position) : base(position)
        { }

    }
}
