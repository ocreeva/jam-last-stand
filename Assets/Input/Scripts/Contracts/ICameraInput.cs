namespace Moyba.Input
{
    public interface ICameraInput
    {
        float Zoom { get; }

        event ValueEventHandler<float> OnZoomChanged;
    }
}
