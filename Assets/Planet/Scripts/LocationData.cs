using UnityEngine;

namespace Moyba.Planet
{
    [CreateAssetMenu(fileName = "NewLocationData", menuName = "Data/Location")]
    public class LocationData : ScriptableObject, ILocationData
    {
        [Header("Configuration")]
        [SerializeField] private Location _location;
        [SerializeField] private string _displayName;

        public Location Location => _location;

        public string DisplayName => _displayName;

        public float Infrastructure { get; internal set; } = 1f;
        public float Defenses { get; internal set; } = 0f;

        public Activity Activity { get; internal set; }
        public Location ActivityLocation { get; internal set; }
    }
}
