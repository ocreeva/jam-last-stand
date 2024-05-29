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
            var locationData = Omnibus.Planet.GetLocationData(location);

            _name.text = locationData.DisplayName;

            _UpdatePercentageComponent(locationData.Infrastructure, _infrastructure);
            _UpdatePercentageComponent(locationData.Defenses, _defenses);
        }

        private void OnDisable()
        {
            Omnibus.Planet.Target.OnLocationChanged -= this.HandleTargetLocationChanged;
        }

        private void OnEnable()
        {
            Omnibus.Planet.Target.OnLocationChanged += this.HandleTargetLocationChanged;
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
