namespace GJ2022.Entities.Turfs.Standard.Floors
{
    public abstract class Floor : Turf
    {

        protected Floor(int x, int y) : base(x, y)
        { }

        public override bool AllowAtmosphericFlow => true;

    }
}
