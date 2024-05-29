namespace Moyba.Planet
{
    public interface IPlanetTime
    {
        int Day { get; }
        bool IsPaused { get; }

        event ValueEventHandler<int> OnDayChanged;
        event SimpleEventHandler OnPause;
        event SimpleEventHandler OnResume;
    }
}
