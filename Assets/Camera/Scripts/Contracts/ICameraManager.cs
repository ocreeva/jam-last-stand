namespace Moyba.Camera
{
    public interface ICameraManager
    {
        ICameraFraming Framing { get; }
        ICameraZoom Zoom { get; }
    }
}
