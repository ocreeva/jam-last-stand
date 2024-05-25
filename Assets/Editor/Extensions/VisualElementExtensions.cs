using UnityEngine.UIElements;

internal static class VisualElementExtensions
{
    public static void Align(this VisualElement element)
        => element.AddToClassList("unity-base-field__aligned");
}
