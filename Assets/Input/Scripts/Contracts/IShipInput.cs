namespace Moyba.Input
{
    public interface IShipInput
    {
        float Turn { get; }

        event ValueEventHandler<float> OnTurnChanged;
    }
}
