using System;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Planet
{
    [CreateAssetMenu(fileName = "NewLocationData", menuName = "Data/Location")]
    public class LocationData : ScriptableObjectContract, ILocationData
    {
        [Header("Configuration")]
        [SerializeField] private Location _location;
        [SerializeField] private string _displayName;

        [NonSerialized] private float _infrastructure = 1f;
        [NonSerialized] private float _defenses = 0f;
        [NonSerialized] private Activity _activity = Activity.Repair;

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

        public event ValueEventHandler<float> OnInfrastructureChanged;
        public event ValueEventHandler<float> OnDefensesChanged;
        public event ValueEventHandler<Activity> OnActivityChanged;
    }
}
