using System;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Ship
{
    public class ShipMovement : TraitBase<ShipManager>, IShipMovement
    {
        private const float _TurnRate = -180 / Mathf.PI;

        private static readonly _StubShipMovement _Stub = new _StubShipMovement();

        [SerializeField, Range(float.Epsilon, 10f)] private float _turnSpeed = 1f;

        internal static IShipMovement Stub => _Stub;

        private void Awake()
        {
            this._Assert(ReferenceEquals(_manager.Movement, _Stub), "is replacing a non-stub instance.");

            _manager.Movement = this;

            _Stub.TransferControlTo(this);
        }

        private void OnDestroy()
        {
            this._Assert(ReferenceEquals(_manager.Movement, this), "is stubbing a different instance.");

            _manager.Movement = _Stub;

            _Stub.TransferControlFrom(this);
        }

        private void FixedUpdate()
        {
            this.FixedUpdate_Rotate();
        }

        private void FixedUpdate_Rotate()
        {
            var turn = Omnibus.Input.Ship.Turn;
            if (Mathf.Abs(turn) < float.Epsilon) return;

            this.transform.Rotate(this.transform.forward, Time.fixedDeltaTime * _TurnRate * _turnSpeed * turn);
        }

        private class _StubShipMovement : TraitStubBase<ShipMovement>, IShipMovement { }
    }
}
