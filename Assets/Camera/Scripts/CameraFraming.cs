using System;
using Moyba.Contracts;
using Unity.Cinemachine;
using UnityEngine;

namespace Moyba.Camera
{
    [RequireComponent(typeof(CinemachinePositionComposer))]
    public class CameraFraming : TraitBase<CameraManager>, ICameraFraming
    {
        private static readonly _StubCameraFraming _Stub = new _StubCameraFraming();

        [Header("Components")]
        [SerializeField] private Transform _target;

        [Header("Configuration")]
        [SerializeField, Range(float.Epsilon, 0.5f)] private float _offsetFromCenter = 0.35f;

        [NonSerialized] private CinemachinePositionComposer _positionComposer;
        [NonSerialized] private Vector2 _originalCompositionScreenPosition;

        internal static ICameraFraming Stub => _Stub;

        private void Awake()
        {
            this._Assert(ReferenceEquals(_manager.Framing, _Stub), "is replacing a non-stub instance.");

            _manager.Framing = this;

            _Stub.TransferControlTo(this);

            _positionComposer = this.GetComponent<CinemachinePositionComposer>();
            _originalCompositionScreenPosition = _positionComposer.Composition.ScreenPosition;
        }

        private void OnDestroy()
        {
            this._Assert(ReferenceEquals(_manager.Framing, this), "is stubbing a different instance.");

            _manager.Framing = _Stub;

            _Stub.TransferControlFrom(this);

            _positionComposer.Composition.ScreenPosition = _originalCompositionScreenPosition;
        }

        private void Update()
        {
            var up = _target.up;
            _positionComposer.Composition.ScreenPosition = new Vector2(
                -up.x * _offsetFromCenter,
                up.y * _offsetFromCenter);
        }

        private class _StubCameraFraming : TraitStubBase<CameraFraming>, ICameraFraming { }
    }
}
