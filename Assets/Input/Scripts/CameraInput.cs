using System;
using Moyba.Contracts;
using UnityEngine.InputSystem;

namespace Moyba.Input
{
    public class CameraInput : TraitBase<InputManager>, ICameraInput
    {
        private static readonly _StubCameraInput _Stub = new _StubCameraInput();

        [NonSerialized] private Controls.CameraActions _cameraActions;

        [NonSerialized] private float _zoom;

        internal static ICameraInput Stub => _Stub;

        public float Zoom
        {
            get => _zoom;
            set => _Set(value, ref _zoom, changed: this.OnZoomChanged);
        }

        public event ValueEventHandler<float> OnZoomChanged;

        private void Awake()
        {
            this._Assert(ReferenceEquals(_manager.Camera, _Stub), "is replacing a non-stub instance.");

            _manager.Camera = this;

            _Stub.TransferControlTo(this);
        }

        private void OnDestroy()
        {
            this._Assert(ReferenceEquals(_manager.Camera, this), "is stubbing a different instance.");

            _manager.Camera = _Stub;

            _Stub.TransferControlFrom(this);
        }

        private void OnDisable()
        {
            _cameraActions.Zoom.canceled -= this.HandleZoomChanged;
            _cameraActions.Zoom.started -= this.HandleZoomChanged;

            _cameraActions.Disable();

            this.Zoom = 0f;
        }

        private void OnEnable()
        {
            _cameraActions = _manager.Controls.Camera;
            _cameraActions.Enable();

            _cameraActions.Zoom.canceled += this.HandleZoomChanged;
            _cameraActions.Zoom.started += this.HandleZoomChanged;
        }

        private void HandleZoomChanged(InputAction.CallbackContext context)
            => this.Zoom = context.ReadValue<float>();

        private class _StubCameraInput : TraitStubBase<CameraInput>, ICameraInput
        {
            public float Zoom => 0f;

            public event ValueEventHandler<float> OnZoomChanged;

            protected override void TransferEvents(CameraInput trait)
            {
                (this.OnZoomChanged, trait.OnZoomChanged) = (trait.OnZoomChanged, this.OnZoomChanged);
            }
        }
    }
}
