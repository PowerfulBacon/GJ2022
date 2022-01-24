namespace GJ2022.Entities.Turfs.Standard.Solids
{
    class Wall : Solid
    {

        protected override string Texture { get; } = "wall";

        public Wall(int x, int y) : base(x, y)
        {
        }

    }
}
