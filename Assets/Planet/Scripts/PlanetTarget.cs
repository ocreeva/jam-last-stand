using System;
using System.Linq;
using Moyba.Contracts;

namespace Moyba.Planet
{
    public class PlanetTarget : TraitBase<PlanetManager>, IPlanetTarget
    {
        private static readonly _StubPlanetTarget _Stub = new _StubPlanetTarget();

        [NonSerialized] private Location[] _locations;
        [NonSerialized] private int _locationIndex;
        [NonSerialized] private Location _location;

        internal static IPlanetTarget Stub => _Stub;

        public Location Location
        {
            get => _location;
            private set => _Set(value, ref _location, changed: this.OnLocationChanged, changing: this.OnLocationChanging);
        }

        private int LocationIndex
        {
            get => _locationIndex;
            set
            {
                _locationIndex = (value + _locations.Length) % _locations.Length;
                this.Location = _locations[_locationIndex];
            }
        }

        public event ValueEventHandler<Location> OnLocationChanging;
        public event ValueEventHandler<Location> OnLocationChanged;

        internal void SetLocation(Location location)
        {
            for (var index = 0; index < _locations.Length; index++)
            {
                if (_locations[index] == location)
                {
                    this.LocationIndex = index;
                    break;
                }
            }
        }

        public void SetToNextLocation()
            => this.LocationIndex++;

        public void SetToPreviousLocation()
            => this.LocationIndex--;

        private void Awake()
        {
            _locations = _manager.GetLocations().ToArray();

            this._Assert(ReferenceEquals(_manager.Target, _Stub), "is replacing a non-stub instance.");

            _manager.Target = this;

            _Stub.TransferControlTo(this);
        }

        private void OnDestroy()
        {
            this._Assert(ReferenceEquals(_manager.Target, this), "is stubbing a different instance.");

            _manager.Target = _Stub;

            _Stub.TransferControlFrom(this);
        }

        private class _StubPlanetTarget : TraitStubBase<PlanetTarget>, IPlanetTarget
        {
            private int? _locationIndex;
            private Location _location;

            public Location Location => _location;

            public event ValueEventHandler<Location> OnLocationChanging;
            public event ValueEventHandler<Location> OnLocationChanged;

            public override void TransferControlFrom(PlanetTarget trait)
            {
                base.TransferControlFrom(trait);

                _location = trait._location;
                _locationIndex = trait._locationIndex;
            }

            public override void TransferControlTo(PlanetTarget trait)
            {
                base.TransferControlTo(trait);

                if (_locationIndex.HasValue)
                {
                    trait._locationIndex = _locationIndex.Value;
                    trait._location = trait._locations[trait._locationIndex];
                }
                else
                {
                    trait.LocationIndex = _locationIndex.GetValueOrDefault(0);
                }
            }

            protected override void TransferEvents(PlanetTarget trait)
            {
                (this.OnLocationChanging, trait.OnLocationChanging) = (trait.OnLocationChanging, this.OnLocationChanging);
                (this.OnLocationChanged, trait.OnLocationChanged) = (trait.OnLocationChanged, this.OnLocationChanged);
            }
        }
    }
}
