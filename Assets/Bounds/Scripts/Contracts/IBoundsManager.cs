namespace Moyba.Bounds
{
    public interface IBoundsManager
    {
        int WarningDistance { get; }
        int AlertDistance { get; }
        int MaximumDistance { get; }
    }
}
