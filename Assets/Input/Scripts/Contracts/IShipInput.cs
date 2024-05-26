namespace Moyba.Input
{
    public interface IShipInput
    {
        float Move { get; }
        float Turn { get; }

        event ValueEventHandler<float> OnMoveChanged;
        event ValueEventHandler<float> OnTurnChanged;
    }
}
