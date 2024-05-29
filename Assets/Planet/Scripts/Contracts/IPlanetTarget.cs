namespace Moyba.Planet
{
    public interface IPlanetTarget
    {
        Location Location { get; }

        event ValueEventHandler<Location> OnLocationChanged;
    }
}
