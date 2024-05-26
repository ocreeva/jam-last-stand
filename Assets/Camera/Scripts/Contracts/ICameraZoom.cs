namespace Moyba.Camera
{
    public interface ICameraZoom
    {
        float Factor { get; }

        event ValueEventHandler<float> OnFactorChanged;
    }
}
