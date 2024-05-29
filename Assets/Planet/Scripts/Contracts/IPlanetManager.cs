namespace Moyba.Planet
{
    public interface IPlanetManager
    {
        IPlanetTarget Target { get; }
        IPlanetTime Time { get; }

        ILocationData GetLocationData(Location location);
    }
}
