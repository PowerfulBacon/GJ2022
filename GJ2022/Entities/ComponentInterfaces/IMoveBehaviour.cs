using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.ComponentInterfaces
{
    public interface IMoveBehaviour
    {

        void OnMoved(Vector<float> oldPosition);
        void OnMoved(Entity oldLocation);

    }
}
