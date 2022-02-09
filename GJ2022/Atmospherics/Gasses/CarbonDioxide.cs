namespace GJ2022.Atmospherics.Gasses
{
    class CarbonDioxide : Gas
    {

        public static CarbonDioxide Singleton { get; } = new CarbonDioxide();

        public override string OverlayTexture => "hydrogen";
    }
}
