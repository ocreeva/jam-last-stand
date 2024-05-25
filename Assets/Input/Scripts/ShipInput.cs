using System;
using Moyba.Contracts;
using UnityEngine.InputSystem;

namespace Moyba.Input
{
    public class ShipInput : TraitBase<InputManager>, IShipInput
    {
        private static readonly _StubShipInput _Stub = new _StubShipInput();

        [NonSerialized] private Controls.ShipActions _shipActions;

        [NonSerialized] private float _turn;

        public float Turn
        {
            get => _turn;
            set => _Set(value, ref _turn, changed: OnTurnChanged);
        }

        public event ValueEventHandler<float> OnTurnChanged;

        internal static IShipInput Stub => _Stub;

        private void Awake()
        {
            this._Assert(ReferenceEquals(_manager.Ship, _Stub), "is replacing a non-stub instance.");

            _manager.Ship = this;

            _Stub.TransferControlTo(this);
        }

        private void OnDestroy()
        {
            this._Assert(ReferenceEquals(_manager.Ship, this), "is stubbing a different instance.");

            _manager.Ship = _Stub;

            _Stub.TransferControlFrom(this);
        }

        private void OnDisable()
        {
            _shipActions.Turn.canceled -= this.HandleTurnChanged;
            _shipActions.Turn.started -= this.HandleTurnChanged;

            _shipActions.Disable();

            this.Turn = 0f;
        }

        private void OnEnable()
        {
            _shipActions = _manager.Controls.Ship;
            _shipActions.Enable();

            _shipActions.Turn.canceled += this.HandleTurnChanged;
            _shipActions.Turn.started += this.HandleTurnChanged;
        }

        private void HandleTurnChanged(InputAction.CallbackContext context)
            => this.Turn = context.ReadValue<float>();

        private class _StubShipInput : TraitStubBase<ShipInput>, IShipInput
        {
            public float Turn => 0f;

            public event ValueEventHandler<float> OnTurnChanged;

            protected override void TransferEvents(ShipInput trait)
            {
                (this.OnTurnChanged, trait.OnTurnChanged) = (trait.OnTurnChanged, this.OnTurnChanged);
            }
        }
    }
}
