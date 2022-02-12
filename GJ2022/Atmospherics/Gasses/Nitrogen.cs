namespace GJ2022.Atmospherics.Gasses
{
    public class Nitrogen : Gas
    {

        public static Nitrogen Singleton { get; } = new Nitrogen();

        public override string OverlayTexture => "oxygen";
    }
}
