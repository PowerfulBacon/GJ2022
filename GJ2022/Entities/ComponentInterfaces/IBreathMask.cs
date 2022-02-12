using GJ2022.Atmospherics;

namespace GJ2022.Entities.ComponentInterfaces
{
    public interface IBreathMask : IEquippable
    {

        Atmosphere GetBreathSource();

    }
}
