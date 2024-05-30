using System;
using System.Linq;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Planet
{
    public class PlanetTime : TraitBase<PlanetManager>, IPlanetTime
    {
        private static readonly _StubPlanetTime _Stub = new _StubPlanetTime();

        [SerializeField] private TimeData _timeData;

        [Header("Configuration")]
        [SerializeField, Range(0f, 1f)] private float _assistEffectiveness = 0.5f;
        [SerializeField, Range(1f, 10f)] private float _secondsPerDay = 1f;

        [NonSerialized] private float _elapsedTime;
        [NonSerialized] private bool _isPaused = true;

        internal static IPlanetTime Stub => _Stub;

        public int Day
        {
            get => _timeData.Day;
            set => _timeData.Day = value;
        }

        public bool IsPaused
        {
            get => _isPaused;
            set => _Set(value, ref _isPaused, onTrue: this.OnPause, onFalse: this.OnResume);
        }

        public event ValueEventHandler<int> OnDayChanged;

        public event SimpleEventHandler OnPause;
        public event SimpleEventHandler OnResume;

        public void Pause()
            => this.IsPaused = true;

        public void TogglePause()
            => this.IsPaused = !this.IsPaused;

        internal void AdvanceDay()
            => this.Day++;

        private void Awake()
        {
            this._Assert(ReferenceEquals(_manager.Time, _Stub), "is replacing a non-stub instance.");

            _manager.Time = this;

            _Stub.TransferControlTo(this);
        }

        private void HandleTimeDataDayChanged(UnityEngine.Object _, int day)
        {
            this.OnDayChanged?.Invoke(this, day);

            var allLocationData = _manager.GetLocations()
                .Select(location => _manager.GetLocationData(location))
                .ToArray();
            var needAssistance = Math.Max(1, allLocationData.Count(data => data.Activity != Activity.Assist));
            var assistance = allLocationData
                .Where(data => data.Activity == Activity.Assist)
                .Select(data => data.Infrastructure * _assistEffectiveness)
                .Sum() / needAssistance;

            foreach (var location in _manager.GetLocations())
            {
                var locationData = _manager.GetLocationData(location);
                locationData.ApplyActivity(assistance);
            }
        }

        private void OnDestroy()
        {
            this._Assert(ReferenceEquals(_manager.Time, this), "is stubbing a different instance.");

            _manager.Time = _Stub;

            _Stub.TransferControlFrom(this);
        }

        private void OnDisable()
        {
            _timeData.OnDayChanged -= this.HandleTimeDataDayChanged;
        }

        private void OnEnable()
        {
            _timeData.OnDayChanged += this.HandleTimeDataDayChanged;
        }

        private void Update()
        {
            if (_isPaused) return;

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _secondsPerDay)
            {
                _elapsedTime -= _secondsPerDay;
                this.Day++;
            }
        }

        private class _StubPlanetTime : TraitStubBase<PlanetTime>, IPlanetTime
        {
            public int Day => 0;
            public bool IsPaused => false;

            public event ValueEventHandler<int> OnDayChanged;
            public event SimpleEventHandler OnPause;
            public event SimpleEventHandler OnResume;

            protected override void TransferEvents(PlanetTime trait)
            {
                (this.OnDayChanged, trait.OnDayChanged) = (trait.OnDayChanged, this.OnDayChanged);
                (this.OnPause, trait.OnPause) = (trait.OnPause, this.OnPause);
                (this.OnResume, trait.OnResume) = (trait.OnResume, this.OnResume);
            }
        }
    }
}
