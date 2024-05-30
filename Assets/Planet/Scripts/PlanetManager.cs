using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Moyba.Planet
{
    [CreateAssetMenu(fileName = "Omnibus.Planet.asset", menuName = "Omnibus/Planet", order = 1)]
    public class PlanetManager : ScriptableObject, IPlanetManager
    {
        [SerializeField] private LocationData[] _locationData;

        [Header("Configuration")]
        [SerializeField, Range(0f, 1f)] private float _fortifyRate = 1f;
        [SerializeField, Range(0f, 1f)] private float _repairRate = 1f;

        public IPlanetTarget Target { get; internal set; } = PlanetTarget.Stub;
        public IPlanetTime Time { get; internal set; } = PlanetTime.Stub;

        internal float FortifyRate => _fortifyRate;
        internal float RepairRate => _repairRate;

        internal IEnumerable<Location> GetLocations()
            => _locationData.Select(d => d.Location);

        ILocationData IPlanetManager.GetLocationData(Location location)
            => _locationData.Single(d => d.Location == location);
        internal LocationData GetLocationData(Location location)
            => _locationData.Single(d => d.Location == location);
    }
}
