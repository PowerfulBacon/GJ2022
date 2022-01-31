namespace GJ2022.Entities.Turfs.Standard.Solids
{
    public class GoldWall : Solid
    {

        protected override string Texture { get; } = "gold_wall";

        public GoldWall(int x, int y) : base(x, y)
        {
        }

    }
}
