using System;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Planet
{
    public class PlanetTime : TraitBase<PlanetManager>, IPlanetTime
    {
        private static readonly _StubPlanetTime _Stub = new _StubPlanetTime();

        [Header("Configuration")]
        [SerializeField, Range(1f, 10f)] private float _secondsPerDay = 1f;

        [NonSerialized] private int _day;
        [NonSerialized] private float _elapsedTime;
        [NonSerialized] private bool _isPaused = true;

        internal static IPlanetTime Stub => _Stub;

        public int Day
        {
            get => _day;
            set => _Set(value, ref _day, changed: this.OnDayChanged);
        }

        public bool IsPaused
        {
            get => _isPaused;
            set => _Set(value, ref _isPaused, onTrue: this.OnPause, onFalse: this.OnResume);
        }

        public event ValueEventHandler<int> OnDayChanged;
        public event SimpleEventHandler OnPause;
        public event SimpleEventHandler OnResume;

        public void TogglePause()
            => this.IsPaused = !this.IsPaused;

        private void Awake()
        {
            this._Assert(ReferenceEquals(_manager.Time, _Stub), "is replacing a non-stub instance.");

            _manager.Time = this;

            _Stub.TransferControlTo(this);
        }

        private void OnDestroy()
        {
            this._Assert(ReferenceEquals(_manager.Time, this), "is stubbing a different instance.");

            _manager.Time = _Stub;

            _Stub.TransferControlFrom(this);
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
            public int Day => 1;
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
