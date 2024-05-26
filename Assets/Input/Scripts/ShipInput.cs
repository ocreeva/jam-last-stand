using System;
using Moyba.Contracts;
using UnityEngine.InputSystem;

namespace Moyba.Input
{
    public class ShipInput : TraitBase<InputManager>, IShipInput
    {
        private static readonly _StubShipInput _Stub = new _StubShipInput();

        [NonSerialized] private Controls.ShipActions _shipActions;

        [NonSerialized] private float _move;
        [NonSerialized] private float _turn;

        public float Move
        {
            get => _move;
            set => _Set(value, ref _move, changed: this.OnMoveChanged);
        }

        public float Turn
        {
            get => _turn;
            set => _Set(value, ref _turn, changed: this.OnTurnChanged);
        }

        public event ValueEventHandler<float> OnMoveChanged;
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
            _shipActions.Move.canceled -= this.HandleMoveChanged;
            _shipActions.Move.started -= this.HandleMoveChanged;

            _shipActions.Turn.canceled -= this.HandleTurnChanged;
            _shipActions.Turn.started -= this.HandleTurnChanged;

            _shipActions.Disable();

            this.Move = 0f;
            this.Turn = 0f;
        }

        private void OnEnable()
        {
            _shipActions = _manager.Controls.Ship;
            _shipActions.Enable();

            _shipActions.Move.canceled += this.HandleMoveChanged;
            _shipActions.Move.started += this.HandleMoveChanged;

            _shipActions.Turn.canceled += this.HandleTurnChanged;
            _shipActions.Turn.started += this.HandleTurnChanged;
        }

        private void HandleMoveChanged(InputAction.CallbackContext context)
            => this.Move = context.ReadValue<float>();

        private void HandleTurnChanged(InputAction.CallbackContext context)
            => this.Turn = context.ReadValue<float>();

        private class _StubShipInput : TraitStubBase<ShipInput>, IShipInput
        {
            public float Move => 0f;
            public float Turn => 0f;

            public event ValueEventHandler<float> OnMoveChanged;
            public event ValueEventHandler<float> OnTurnChanged;

            protected override void TransferEvents(ShipInput trait)
            {
                (this.OnMoveChanged, trait.OnMoveChanged) = (trait.OnMoveChanged, this.OnMoveChanged);
                (this.OnTurnChanged, trait.OnTurnChanged) = (trait.OnTurnChanged, this.OnTurnChanged);
            }
        }
    }
}
