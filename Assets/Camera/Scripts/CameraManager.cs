using UnityEngine;

namespace Moyba.Camera
{
    [CreateAssetMenu(fileName = "Omnibus.Camera.asset", menuName = "Omnibus/Camera", order = 1)]
    public class CameraManager : ScriptableObject, ICameraManager
    {
        public ICameraFraming Framing { get; internal set; } = CameraFraming.Stub;
    }
}
