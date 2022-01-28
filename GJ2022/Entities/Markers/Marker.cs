using GJ2022.Entities.ComponentInterfaces;
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
        }

        public override bool Destroy()
        {
            Destroyed = true;
            return base.Destroy();
        }

        public abstract bool IsValidPosition();

    }
}
