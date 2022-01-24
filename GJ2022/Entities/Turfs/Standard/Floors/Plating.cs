namespace GJ2022.Entities.Turfs.Standard.Floors
{
    public class Plating : Floor
    {

        protected override string Texture { get; } = "plating";

        public Plating(int x, int y) : base(x, y)
        { }

    }
}
