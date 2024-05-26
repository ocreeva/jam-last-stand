namespace Moyba.Input
{
    public interface IShipInput
    {
        bool ShouldFire { get; }
        float Move { get; }
        float Turn { get; }

        event SimpleEventHandler OnFireStarted;
        event SimpleEventHandler OnFireStopped;
        event ValueEventHandler<float> OnMoveChanged;
        event ValueEventHandler<float> OnTurnChanged;
    }
}
