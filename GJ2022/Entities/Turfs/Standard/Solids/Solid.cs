using GJ2022.Entities.ComponentInterfaces;

namespace GJ2022.Entities.Turfs.Standard.Solids
{
    public abstract class Solid : StandardRenderableTurf, ISolid
    {

        public override bool AllowAtmosphericFlow => false;

        protected Solid(int x, int y) : base(x, y)
        {
        }
    }
}
