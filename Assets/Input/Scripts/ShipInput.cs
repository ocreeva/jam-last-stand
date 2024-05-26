using System;
using Moyba.Contracts;
using UnityEngine.InputSystem;

namespace Moyba.Input
{
    public class ShipInput : TraitBase<InputManager>, IShipInput
    {
        private static readonly _StubShipInput _Stub = new _StubShipInput();

        [NonSerialized] private Controls.ShipActions _shipActions;

        [NonSerialized] private bool _shouldFire;
        [NonSerialized] private float _move;
        [NonSerialized] private float _turn;

        internal static IShipInput Stub => _Stub;

        public bool ShouldFire
        {
            get => _shouldFire;
            set => _Set(value, ref _shouldFire, onFalse: this.OnFireStopped, onTrue: this.OnFireStarted);
        }

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

        public event SimpleEventHandler OnFireStarted;
        public event SimpleEventHandler OnFireStopped;
        public event ValueEventHandler<float> OnMoveChanged;
        public event ValueEventHandler<float> OnTurnChanged;

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
            _shipActions.Fire.canceled -= this.HandleFireCanceled;
            _shipActions.Fire.started -= this.HandleFireStarted;

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

            _shipActions.Fire.canceled += this.HandleFireCanceled;
            _shipActions.Fire.started += this.HandleFireStarted;

            _shipActions.Move.canceled += this.HandleMoveChanged;
            _shipActions.Move.started += this.HandleMoveChanged;

            _shipActions.Turn.canceled += this.HandleTurnChanged;
            _shipActions.Turn.started += this.HandleTurnChanged;
        }

        private void HandleFireCanceled(InputAction.CallbackContext _)
            => this.ShouldFire = false;
        private void HandleFireStarted(InputAction.CallbackContext _)
            => this.ShouldFire = true;

        private void HandleMoveChanged(InputAction.CallbackContext context)
            => this.Move = context.ReadValue<float>();

        private void HandleTurnChanged(InputAction.CallbackContext context)
            => this.Turn = context.ReadValue<float>();

        private class _StubShipInput : TraitStubBase<ShipInput>, IShipInput
        {
            public bool ShouldFire => false;
            public float Move => 0f;
            public float Turn => 0f;

            public event SimpleEventHandler OnFireStarted;
            public event SimpleEventHandler OnFireStopped;
            public event ValueEventHandler<float> OnMoveChanged;
            public event ValueEventHandler<float> OnTurnChanged;

            protected override void TransferEvents(ShipInput trait)
            {
                (this.OnFireStarted, trait.OnFireStarted) = (trait.OnFireStarted, this.OnFireStarted);
                (this.OnFireStopped, trait.OnFireStopped) = (trait.OnFireStopped, this.OnFireStopped);
                (this.OnMoveChanged, trait.OnMoveChanged) = (trait.OnMoveChanged, this.OnMoveChanged);
                (this.OnTurnChanged, trait.OnTurnChanged) = (trait.OnTurnChanged, this.OnTurnChanged);
            }
        }
    }
}
