using UnityEngine;

namespace Moyba.Bounds
{
    [CreateAssetMenu(fileName = "Omnibus.Bounds.asset", menuName = "Omnibus/Bounds", order = 1)]
    public class BoundsManager : ScriptableObject, IBoundsManager
    {
        [Header("Configuration")]
        [SerializeField, Range(1, 1022)] private int _warningDistance = 10;
        [SerializeField, Range(2, 1023)] private int _alertDistance = 11;
        [SerializeField, Range(3, 1024)] private int _maximumDistance = 12;

        public int WarningDistance => _warningDistance;
        public int AlertDistance => _alertDistance;
        public int MaximumDistance => _maximumDistance;

        internal BoundsAppendix Appendix { get; set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_alertDistance <= _warningDistance) _alertDistance = _warningDistance + 1;
            if (_maximumDistance <= _alertDistance) _maximumDistance = _alertDistance + 1;
        }
#endif
    }
}
