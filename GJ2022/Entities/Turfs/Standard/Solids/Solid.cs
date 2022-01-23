using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Turfs.Standard.Solids
{
    public abstract class Solid : StandardRenderableTurf
    {
        protected Solid(int x, int y) : base(x, y)
        {
        }
    }
}
