using System;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Ship
{
    public class ShipMovement : TraitBase<ShipManager>, IShipMovement
    {
        private static readonly _StubShipMovement _Stub = new _StubShipMovement();

        [Header("Configuration")]
        [SerializeField, Range(float.Epsilon, 10f)] private float _accelerationRate = 1f;
        [SerializeField, Range(float.Epsilon, 10f)] private float _maximumVelocity = 1f;
        [SerializeField, Range(float.Epsilon, 10f)] private float _rotationRate = 1f;

        [NonSerialized] private Vector3 _velocity;

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

        private void OnDisable()
        {
            Omnibus.Input.Ship.OnTurnChanged -= this.HandleTurnChanged;
            this.HandleTurnChanged(this, 0f);
        }

        private void OnEnable()
        {
            Omnibus.Input.Ship.OnTurnChanged += this.HandleTurnChanged;
            this.HandleTurnChanged(this, Omnibus.Input.Ship.Turn);
        }

        private void FixedUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;

            this.Update_Accelerate(deltaTime);
            this.Update_Move(deltaTime);
            this.Update_Rotate(deltaTime);
        }

        private void Update_Accelerate(float deltaTime)
        {
            var move = Omnibus.Input.Ship.Move;
            if (Mathf.Abs(move) < float.Epsilon) return;

            _velocity = Vector3.ClampMagnitude(_velocity + this.transform.up * deltaTime * _accelerationRate * move, _maximumVelocity);
        }

        private void Update_Move(float deltaTime)
        {
            this.transform.position += _velocity * deltaTime;
        }

        private void Update_Rotate(float deltaTime)
        {
            var turn = Omnibus.Input.Ship.Turn;
            if (Mathf.Abs(turn) < float.Epsilon) return;

            this.transform.Rotate(this.transform.forward, deltaTime * Mathf.Rad2Deg * _rotationRate * turn);
        }

        private void HandleTurnChanged(UnityEngine.Object _, float turn)
            => Omnibus.Camera.Framing.enabled = Math.Abs(turn) > float.Epsilon;

        private class _StubShipMovement : TraitStubBase<ShipMovement>, IShipMovement { }
    }
}
