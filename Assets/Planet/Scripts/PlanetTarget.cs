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
            this._Assert(ReferenceEquals(_manager.Target, _Stub), "is replacing a non-stub instance.");

            _manager.Target = this;

            _Stub.TransferControlTo(this);

            _locations = _manager.GetLocations().ToArray();
        }

        private void OnDestroy()
        {
            this._Assert(ReferenceEquals(_manager.Target, this), "is stubbing a different instance.");

            _manager.Target = _Stub;

            _Stub.TransferControlFrom(this);
        }

        private void Start()
        {
            this.LocationIndex = 0;
        }

        private class _StubPlanetTarget : TraitStubBase<PlanetTarget>, IPlanetTarget
        {
            public Location Location { get; private set; } = Location.NorthAmerica;

            public event ValueEventHandler<Location> OnLocationChanging;
            public event ValueEventHandler<Location> OnLocationChanged;

            public override void TransferControlFrom(PlanetTarget trait)
            {
                base.TransferControlFrom(trait);

                this.Location = trait.Location;
            }

            public override void TransferControlTo(PlanetTarget trait)
            {
                base.TransferControlTo(trait);

                trait.Location = this.Location;
            }

            protected override void TransferEvents(PlanetTarget trait)
            {
                (this.OnLocationChanging, trait.OnLocationChanging) = (trait.OnLocationChanging, this.OnLocationChanging);
                (this.OnLocationChanged, trait.OnLocationChanged) = (trait.OnLocationChanged, this.OnLocationChanged);
            }
        }
    }
}
