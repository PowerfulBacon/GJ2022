namespace GJ2022.Entities.Turfs.Standard.Floors
{
    class GoldFloor : Floor
    {

        protected override string Texture { get; } = "gold_floor";

        public GoldFloor(int x, int y) : base(x, y)
        { }

    }
}
