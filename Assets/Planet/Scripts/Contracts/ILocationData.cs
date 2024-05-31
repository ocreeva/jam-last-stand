namespace Moyba.Planet
{
    public interface ILocationData
    {
        Location Location { get; }

        string DisplayName { get; }

        float Infrastructure { get; }
        float Defenses { get; }

        Activity Activity { get; }

        int AsteroidCount { get; set; }

        event ValueEventHandler<float> OnInfrastructureChanged;
        event ValueEventHandler<float> OnDefensesChanged;
        event ValueEventHandler<Activity> OnActivityChanged;
        event ValueEventHandler<int> OnAsteroidCountChanged;
    }
}
