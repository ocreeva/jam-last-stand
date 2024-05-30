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

        [Header("Configuration")]
        [SerializeField] private Color _healthyColor = Color.green;
        [SerializeField] private Color _damagedColor = Color.yellow;
        [SerializeField] private Color _criticalColor = Color.red;
        [SerializeField] private Color _zeroColor = Color.gray;

        private void HandleTargetLocationChanged(UnityEngine.Object _, Location location)
        {
            if (location == Location.None) return;

            var locationData = Omnibus.Planet.GetLocationData(location);

            locationData.OnActivityChanged += this.HandleLocationActivityChanged;
            locationData.OnDefensesChanged += this.HandleLocationDefensesChanged;
            locationData.OnInfrastructureChanged += this.HandleLocationInfrastructureChanged;

            _name.text = locationData.DisplayName;

            _UpdatePercentageComponent(locationData.Infrastructure, _infrastructure);
            _UpdatePercentageComponent(locationData.Defenses, _defenses);

            this.UpdateActivity(locationData.Activity);
        }

        private void HandleTargetLocationChanging(UnityEngine.Object _, Location location)
        {
            if (location == Location.None) return;

            var locationData = Omnibus.Planet.GetLocationData(location);

            locationData.OnActivityChanged -= this.HandleLocationActivityChanged;
            locationData.OnDefensesChanged -= this.HandleLocationDefensesChanged;
            locationData.OnInfrastructureChanged -= this.HandleLocationInfrastructureChanged;
        }

        private void HandleLocationActivityChanged(UnityEngine.Object _, Activity activity)
            => this.UpdateActivity(activity);

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
            }
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
                    _target.text = "Critical Infrastructure";
                    break;

                default:
                    throw new ArgumentException($"Unhandled {nameof(Activity)} value: {activity}", nameof(activity));
            }
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
                > 0.8f => _healthyColor,
                > 0.4f => _damagedColor,
                > float.Epsilon => _criticalColor,
                _ => _zeroColor
            };
    }
}
