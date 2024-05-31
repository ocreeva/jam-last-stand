using System;
using Moyba.Contracts;
using UnityEngine;
using UnityEngine.UI;

namespace Moyba.Planet.UI
{
    public class LocationHighlight : MonoBehaviour
    {
        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _alertSprite;

        [Header("Components")]
        [SerializeField] private PlanetTarget _planetTarget;
        [SerializeField] private Image _image;

        [Header("Configuration")]
        [SerializeField] private bool _useColorValueOverride;
        [SerializeField, Range(0f, 1f)] private float _colorValueOverride;
        [SerializeField] private Location _location;

        [NonSerialized] private ILocationData _locationData;

        public void SetTargetLocation()
        {
            _planetTarget.SetLocation(_location);
        }

        private void Awake()
        {
            _locationData = Omnibus.Planet.GetLocationData(_location);
        }

        private void DisplayAlertHighlight()
        {
            _image.sprite = _alertSprite;
            _image.color = this.GetAdjustedColor(Omnibus.AlertStatusColor);
        }

        private void DisplayNormalHighlight()
        {
            _image.sprite = _normalSprite;

            var color = _locationData.Infrastructure switch
            {
                > 0.8f => Omnibus.HealthyStatusColor,
                > 0.4f => Omnibus.DamagedStatusColor,
                > 0f => Omnibus.CriticalStatusColor,
                _ => Omnibus.NoStatusColor
            };
            _image.color = this.GetAdjustedColor(color);
        }

        private Color GetAdjustedColor(Color color)
        {
            if (!_useColorValueOverride) return color;

            Color.RGBToHSV(color, out float h, out float s, out float _);
            return Color.HSVToRGB(h, s, _colorValueOverride);
        }

        private void HandleLocationValueChanged<T>(UnityEngine.Object _, T value)
        {
            if (_locationData.AsteroidCount > 0)
            {
                this.DisplayAlertHighlight();
            }
            else
            {
                this.DisplayNormalHighlight();
            }
        }

        private void OnDisable()
        {
            _locationData.OnAsteroidCountChanged -= this.HandleLocationValueChanged;
            _locationData.OnInfrastructureChanged -= this.HandleLocationValueChanged;
        }

        private void OnEnable()
        {
            _locationData.OnAsteroidCountChanged += this.HandleLocationValueChanged;
            _locationData.OnInfrastructureChanged += this.HandleLocationValueChanged;

            this.HandleLocationValueChanged(this, 1);
        }
    }
}
