namespace GJ2022.Atmospherics.Gasses
{
    public class Oxygen : Gas
    {

        public static Oxygen Singleton { get; } = new Oxygen();

        public override string OverlayTexture => "oxygen";

    }
}
