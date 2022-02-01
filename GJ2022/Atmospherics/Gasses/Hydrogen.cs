namespace GJ2022.Atmospherics.Gasses
{
    public class Hydrogen : Gas
    {

        public static Hydrogen Singleton { get; } = new Hydrogen();

        public override string OverlayTexture => "hydrogen";

    }
}
