using System;
using System.Linq;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Planet
{
    public class PlanetAsteroids : TraitBase<PlanetManager>
    {
        [SerializeField] private PlanetTime _planetTime;

        [Header("Configuration")]
        [SerializeField] private bool _autoPause = true;
        [SerializeField, Range(1, 10)] private int _daysBetween = 1;
        [SerializeField, Range(0, 10)] private int _daysBetweenVariance = 0;
        [SerializeField, Range(1, 4)] private int _locationCount = 2;
        [SerializeField, Range(1, 100)] private int _asteroidCount = 10;
        [SerializeField, Range(0, 100)] private int _asteroidCountVariance = 0;

        [NonSerialized] private int _daysUntil;

        private void HandleDayChanged(UnityEngine.Object _, int day)
        {
            if (--_daysUntil > 0) return;

            this.CalculateNextDaysUntil();

            var locations = _manager.GetLocations().ToList();
            for (var iteration = 0; iteration < _locationCount && locations.Count > 0; iteration++)
            {
                var index = UnityEngine.Random.Range(0, locations.Count - 1);
                var location = locations[index];
                var locationData = _manager.GetLocationData(location);
                locationData.AsteroidCount = UnityEngine.Random.Range(_asteroidCount, _asteroidCount + _asteroidCountVariance);

                locations.RemoveAt(index);
            }

            if (_autoPause) _planetTime.Pause();
        }

        private void OnDisable()
        {
            Omnibus.Planet.Time.OnDayChanged -= this.HandleDayChanged;
        }

        private void OnEnable()
        {
            Omnibus.Planet.Time.OnDayChanged += this.HandleDayChanged;
        }

        private void Start()
        {
            this.CalculateNextDaysUntil();
        }

        private void CalculateNextDaysUntil()
        {
            _daysUntil = UnityEngine.Random.Range(_daysBetween, _daysBetween + _daysBetweenVariance);
        }
    }
}
