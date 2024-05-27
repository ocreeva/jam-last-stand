using System;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Fx
{
    public class CombatBounds : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CombatBoundsPanel _northBounds;
        [SerializeField] private CombatBoundsPanel _westBounds;
        [SerializeField] private CombatBoundsPanel _eastBounds;
        [SerializeField] private CombatBoundsPanel _southBounds;

        [Header("Configuration")]
        [SerializeField, Range(0f, 1024f)] private float _hazeWarningDistance;
        [SerializeField, Range(0f, 1024f)] private float _hazeAlertDistance;
        [SerializeField, Range(0f, 1024f)] private float _maximumDistance;

        private void Update()
        {
            var shipPosition = Omnibus.Ship.Position;
            this.Update_CheckBounds(shipPosition.y, _northBounds);
            this.Update_CheckBounds(-shipPosition.y, _southBounds);
            this.Update_CheckBounds(-shipPosition.x, _westBounds);
            this.Update_CheckBounds(shipPosition.x, _eastBounds);
        }

        private void Update_CheckBounds(float distance, CombatBoundsPanel boundsPanel)
        {
            boundsPanel.ShouldShowAlert = distance > _hazeAlertDistance;
            boundsPanel.ShouldShowWarning = distance > _hazeWarningDistance;

            if (distance > _maximumDistance) Debug.LogError("Ship out of bounds -- do something here!");
        }
    }
}
