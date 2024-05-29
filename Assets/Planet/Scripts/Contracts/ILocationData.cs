namespace Moyba.Planet
{
    public interface ILocationData
    {
        Location Location { get; }

        string DisplayName { get; }

        float Infrastructure { get; }
        float Defenses { get; }

        Activity Activity { get; }
        Location ActivityLocation { get; }
    }
}
