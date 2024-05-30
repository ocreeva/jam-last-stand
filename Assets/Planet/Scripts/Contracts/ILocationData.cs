namespace Moyba.Planet
{
    public interface ILocationData
    {
        Location Location { get; }

        string DisplayName { get; }

        float Infrastructure { get; }
        float Defenses { get; }

        Activity Activity { get; }

        event ValueEventHandler<float> OnInfrastructureChanged;
        event ValueEventHandler<float> OnDefensesChanged;
        event ValueEventHandler<Activity> OnActivityChanged;
    }
}
