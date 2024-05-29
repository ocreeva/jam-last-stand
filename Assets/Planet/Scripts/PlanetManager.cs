using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Moyba.Planet
{
    [CreateAssetMenu(fileName = "Omnibus.Planet.asset", menuName = "Omnibus/Planet", order = 1)]
    public class PlanetManager : ScriptableObject, IPlanetManager
    {
        [SerializeField] private LocationData[] _locationData;

        public IPlanetTarget Target { get; internal set; } = PlanetTarget.Stub;
        public IPlanetTime Time { get; internal set; } = PlanetTime.Stub;

        internal IEnumerable<Location> GetLocations()
            => _locationData.Select(d => d.Location);

        ILocationData IPlanetManager.GetLocationData(Location location)
            => _locationData.Single(d => d.Location == location);
        internal LocationData GetLocationData(Location location)
            => _locationData.Single(d => d.Location == location);
    }
}
