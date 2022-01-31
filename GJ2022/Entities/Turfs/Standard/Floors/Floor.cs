namespace GJ2022.Entities.Turfs.Standard.Floors
{
    public abstract class Floor : StandardRenderableTurf
    {

        public override bool AllowAtmosphericFlow => true;

        protected Floor(int x, int y) : base(x, y)
        { }
    }
}
