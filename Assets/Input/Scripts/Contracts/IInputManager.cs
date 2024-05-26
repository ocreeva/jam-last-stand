namespace Moyba.Input
{
    public interface IInputManager
    {
        ICameraInput Camera { get; }
        IShipInput Ship { get; }
    }
}
