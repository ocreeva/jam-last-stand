using System;
using Moyba.Contracts;
using UnityEngine;
using UnityEngine.UI;

namespace Moyba.Bounds
{
    public class BoundsPanel : EntityBase<BoundsManager>
    {
        [Header("Components")]
        [SerializeField] private Image[] _hazeImages;

        [Header("Configuration")]
        [SerializeField] private Color _warningColor = Color.yellow;
        [SerializeField] private Color _alertColor = Color.red;

        [NonSerialized] private bool _shouldShowAlert, _shouldShowWarning;

        internal bool ShouldShowAlert
        {
            get => _shouldShowAlert;
            set => _Set(value, ref _shouldShowAlert, onTrue: this.OnShowAlert, onFalse: this.OnHideAlert);
        }

        internal bool ShouldShowWarning
        {
            get => _shouldShowWarning;
            set => _Set(value, ref _shouldShowWarning, onTrue: this.OnShowWarning, onFalse: this.OnHideWarning);
        }

        public event SimpleEventHandler OnHideAlert, OnHideWarning;
        public event SimpleEventHandler OnShowAlert, OnShowWarning;

        private void Awake()
        {
            foreach (var image in _hazeImages) image.enabled = false;
        }

        private void HandleHideAlert(UnityEngine.Object _)
        {
            foreach (var image in _hazeImages) image.color = _warningColor;
        }

        private void HandleHideWarning(UnityEngine.Object _)
        {
            foreach (var image in _hazeImages) image.enabled = false;
        }

        private void HandleShowAlert(UnityEngine.Object _)
        {
            foreach (var image in _hazeImages) image.color = _alertColor;
        }

        private void HandleShowWarning(UnityEngine.Object _)
        {
            foreach (var image in _hazeImages)
            {
                image.enabled = true;
                image.color = _warningColor;
            }
        }

        private void OnDisable()
        {
            this.OnHideAlert -= this.HandleHideAlert;
            this.OnHideWarning -= this.HandleHideWarning;
            this.OnShowAlert -= this.HandleShowAlert;
            this.OnShowWarning -= this.HandleShowWarning;
        }

        private void OnEnable()
        {
            this.OnHideAlert += this.HandleHideAlert;
            this.OnHideWarning += this.HandleHideWarning;
            this.OnShowAlert += this.HandleShowAlert;
            this.OnShowWarning += this.HandleShowWarning;
        }
    }
}
