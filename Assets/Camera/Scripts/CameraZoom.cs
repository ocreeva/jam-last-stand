using System;
using Moyba.Contracts;
using Unity.Cinemachine;
using UnityEngine;

namespace Moyba.Camera
{
    [RequireComponent(typeof(CinemachinePositionComposer))]
    public class CameraZoom : TraitBase<CameraManager>, ICameraZoom
    {
        private static readonly _StubCameraZoom _Stub = new _StubCameraZoom();

        [Header("Configuration")]
        [SerializeField, Range(0f, 10f)] private float _minimumZoomFactor = 1f;
        [SerializeField, Range(0f, 10f)] private float _maximumZoomFactor = 1f;
        [SerializeField, Range(float.Epsilon, 10f)] private float _zoomRate = 1f;

        [NonSerialized] private CinemachinePositionComposer _positionComposer;

        [NonSerialized] private float _factor;

        internal static ICameraZoom Stub => _Stub;

        public float Factor
        {
            get => _factor;
            set => _Set(value, ref _factor, changed: this.OnFactorChanged);
        }

        public event ValueEventHandler<float> OnFactorChanged;

        private void Awake()
        {
            this._Assert(ReferenceEquals(_manager.Zoom, _Stub), "is replacing a non-stub instance.");

            _manager.Zoom = this;

            _Stub.TransferControlTo(this);

            _positionComposer = this.GetComponent<CinemachinePositionComposer>();
            _factor = Mathf.Log(_positionComposer.CameraDistance);
        }

        private void OnDestroy()
        {
            this._Assert(ReferenceEquals(_manager.Zoom, this), "is stubbing a different instance.");

            _manager.Zoom = _Stub;

            _Stub.TransferControlFrom(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_maximumZoomFactor < _minimumZoomFactor) _maximumZoomFactor = _minimumZoomFactor;
        }
#endif

        private void Update()
        {
            var deltaTime = Time.deltaTime;

            this.Update_Zoom(deltaTime);
        }

        private void Update_Zoom(float deltaTime)
        {
            var zoom = Omnibus.Input.Camera.Zoom;
            if (Math.Abs(zoom) < float.Epsilon) return;

            this.Factor = Mathf.Clamp(this.Factor + zoom * _zoomRate * deltaTime, _minimumZoomFactor, _maximumZoomFactor);
            _positionComposer.CameraDistance = Mathf.Exp(this.Factor);
        }

        private class _StubCameraZoom : TraitStubBase<CameraZoom>, ICameraZoom
        {
            public float Factor => 1f;

            public event ValueEventHandler<float> OnFactorChanged;

            protected override void TransferEvents(CameraZoom trait)
            {
                (this.OnFactorChanged, trait.OnFactorChanged) = (trait.OnFactorChanged, this.OnFactorChanged);
            }
        }
    }
}
