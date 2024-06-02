using System;
using Moyba.Contracts;
using TMPro;
using UnityEngine;

namespace Moyba.Planet.UI
{
    public class InformationPanel : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _infrastructure;
        [SerializeField] private TextMeshProUGUI _defenses;
        [SerializeField] private TextMeshProUGUI _activity;
        [SerializeField] private TextMeshProUGUI _target;
        [SerializeField] private GameObject _defendButton;

        private void HandleTargetLocationChanged(UnityEngine.Object _, Location location)
        {
            if (location == Location.None) return;

            var locationData = Omnibus.Planet.GetLocationData(location);

            locationData.OnActivityChanged += this.HandleLocationActivityChanged;
            locationData.OnDefensesChanged += this.HandleLocationDefensesChanged;
            locationData.OnInfrastructureChanged += this.HandleLocationInfrastructureChanged;
            locationData.OnAsteroidCountChanged += this.HandleLocationAsteroidCountChanged;

            _name.text = locationData.DisplayName;

            _UpdatePercentageComponent(locationData.Infrastructure, _infrastructure);
            _UpdatePercentageComponent(locationData.Defenses, _defenses);

            this.UpdateActivity(locationData.Activity);

            this.UpdateDefendButton(locationData.AsteroidCount);
        }

        private void HandleTargetLocationChanging(UnityEngine.Object _, Location location)
        {
            if (location == Location.None) return;

            var locationData = Omnibus.Planet.GetLocationData(location);

            locationData.OnActivityChanged -= this.HandleLocationActivityChanged;
            locationData.OnDefensesChanged -= this.HandleLocationDefensesChanged;
            locationData.OnInfrastructureChanged -= this.HandleLocationInfrastructureChanged;
            locationData.OnAsteroidCountChanged -= this.HandleLocationAsteroidCountChanged;
        }

        private void HandleLocationActivityChanged(UnityEngine.Object _, Activity activity)
            => this.UpdateActivity(activity);

        private void HandleLocationAsteroidCountChanged(UnityEngine.Object _, int asteroidCount)
            => this.UpdateDefendButton(asteroidCount);

        private void HandleLocationDefensesChanged(UnityEngine.Object _, float defenses)
            => _UpdatePercentageComponent(defenses, _defenses);

        private void HandleLocationInfrastructureChanged(UnityEngine.Object _, float infrastructure)
            => _UpdatePercentageComponent(infrastructure, _infrastructure);

        private void OnDisable()
        {
            Omnibus.Planet.Target.OnLocationChanging -= this.HandleTargetLocationChanging;
            Omnibus.Planet.Target.OnLocationChanged -= this.HandleTargetLocationChanged;

            var location = Omnibus.Planet.Target.Location;
            if (location != Location.None)
            {
                var locationData = Omnibus.Planet.GetLocationData(location);
                locationData.OnActivityChanged -= this.HandleLocationActivityChanged;
                locationData.OnDefensesChanged -= this.HandleLocationDefensesChanged;
                locationData.OnInfrastructureChanged -= this.HandleLocationInfrastructureChanged;
                locationData.OnAsteroidCountChanged -= this.HandleLocationAsteroidCountChanged;
            }
        }

        private void OnEnable()
        {
            Omnibus.Planet.Target.OnLocationChanging += this.HandleTargetLocationChanging;
            Omnibus.Planet.Target.OnLocationChanged += this.HandleTargetLocationChanged;

            var location = Omnibus.Planet.Target.Location;
            if (location != Location.None)
            {
                var locationData = Omnibus.Planet.GetLocationData(location);
                locationData.OnActivityChanged += this.HandleLocationActivityChanged;
                locationData.OnDefensesChanged += this.HandleLocationDefensesChanged;
                locationData.OnInfrastructureChanged += this.HandleLocationInfrastructureChanged;
                locationData.OnAsteroidCountChanged += this.HandleLocationAsteroidCountChanged;
            }
        }

        private void Start()
        {
            var location = Omnibus.Planet.Target.Location;
            var locationData = Omnibus.Planet.GetLocationData(location);

            _name.text = locationData.DisplayName;

            _UpdatePercentageComponent(locationData.Infrastructure, _infrastructure);
            _UpdatePercentageComponent(locationData.Defenses, _defenses);

            this.UpdateActivity(locationData.Activity);
        }

        private void UpdateActivity(Activity activity)
        {
            switch (activity)
            {
                case Activity.Assist:
                    _activity.text = "Assisting";
                    _target.text = "Global Efforts";
                    break;

                case Activity.Fortify:
                    _activity.text = "Fortifying";
                    _target.text = "Region Defenses";
                    break;

                case Activity.Repair:
                    _activity.text = "Repairing";
                    _target.text = "Infrastructure";
                    break;

                default:
                    throw new ArgumentException($"Unhandled {nameof(Activity)} value: {activity}", nameof(activity));
            }
        }

        private void UpdateDefendButton(int asteroidCount)
        {
            _defendButton.SetActive(asteroidCount > 0);
        }

        private void _UpdatePercentageComponent(float value, TextMeshProUGUI component)
        {
            component.text = _GetPercentage(value);
            component.color = _GetPercentageColor(value);
        }

        private static string _GetPercentage(float value)
            => $"{value:P0}";

        private Color _GetPercentageColor(float value)
            => value switch {
                > 0.8f => Omnibus.HealthyStatusColor,
                > 0.4f => Omnibus.DamagedStatusColor,
                > float.Epsilon => Omnibus.CriticalStatusColor,
                _ => Omnibus.NoStatusColor
            };
    }
}
