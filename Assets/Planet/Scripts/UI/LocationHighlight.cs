using UnityEngine;

namespace Moyba.Planet.UI
{
    public class LocationHighlight : MonoBehaviour
    {
        [SerializeField] private Location _location;

        [Header("Components")]
        [SerializeField] private PlanetTarget _planetTarget;

        public void SetTargetLocation()
        {
            _planetTarget.SetLocation(_location);
        }
    }
}
