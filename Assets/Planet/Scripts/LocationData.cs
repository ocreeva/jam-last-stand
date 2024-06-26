using System;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Planet
{
    [CreateAssetMenu(fileName = "NewLocationData", menuName = "Data/Location")]
    public class LocationData : ScriptableObjectContract, ILocationData
    {
        [SerializeField] private PlanetManager _manager;

        [Header("Configuration")]
        [SerializeField] private Location _location;
        [SerializeField] private string _displayName;

        [NonSerialized] private float _infrastructure = 1f;
        [NonSerialized] private float _defenses = 0f;
        [NonSerialized] private Activity _activity = Activity.Repair;
        [NonSerialized] private int _asteroidCount = 10;

        public Location Location => _location;

        public string DisplayName => _displayName;

        public float Infrastructure
        {
            get => _infrastructure;
            set => _Set(value, ref _infrastructure, changed: this.OnInfrastructureChanged);
        }

        public float Defenses
        {
            get => _defenses;
            set => _Set(value, ref _defenses, changed: this.OnDefensesChanged);
        }

        public Activity Activity
        {
            get => _activity;
            set => _Set(value, ref _activity, changed: this.OnActivityChanged);
        }

        public int AsteroidCount
        {
            get => _asteroidCount;
            set => _Set(value, ref _asteroidCount, changed: this.OnAsteroidCountChanged);
        }

        public event ValueEventHandler<float> OnInfrastructureChanged;
        public event ValueEventHandler<float> OnDefensesChanged;
        public event ValueEventHandler<Activity> OnActivityChanged;
        public event ValueEventHandler<int> OnAsteroidCountChanged;

        internal void ApplyActivity(float assistance)
        {
            switch (this.Activity)
            {
                case Activity.Assist:
                    break;

                case Activity.Fortify:
                    this.Defenses = Mathf.Clamp01(this.Defenses + _manager.FortifyRate * (assistance + this.Infrastructure));
                    break;

                case Activity.Repair:
                    this.Infrastructure = Mathf.Clamp01(this.Infrastructure + _manager.RepairRate * (assistance + this.Infrastructure));
                    break;

                default:
                    throw new NotSupportedException($"Unhandled {nameof(Activity)} value: {this.Activity}");
            }
        }

        internal void ApplyDamage(float damage)
        {
            this.Infrastructure = Mathf.Clamp01(this.Infrastructure - damage * (1 - this.Defenses));
        }
    }
}
