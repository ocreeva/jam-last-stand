using System;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Enemies
{
    public class Asteroid : Enemy, IEnemy
    {
        [Header("Configuration")]
        [SerializeField, Range(float.Epsilon, 10f)] private float _speedMin = 1f;
        [SerializeField, Range(0f, 10f)] private float _speedVariance = 0f;

        [NonSerialized] private Vector3 _heading;

        private void FixedUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;

            this.Update_Position(deltaTime);
        }

        private void OnDisable()
        {
            _manager.Deregister(this);
        }

        private void OnEnable()
        {
            _manager.Register(this);
        }

        private void Start()
        {
            var maximumDistance = Omnibus.Bounds.MaximumDistance;
            var warningDistance = Omnibus.Bounds.WarningDistance;
            var targetRange = UnityEngine.Random.value * (maximumDistance + warningDistance) - (this.transform.position.x < 0 ? warningDistance : maximumDistance);
            var target = new Vector3(targetRange, -maximumDistance);

            var speed = _speedMin + UnityEngine.Random.value * _speedVariance;

            _heading = (target - this.transform.position).normalized * speed;
        }

        private void Update_Position(float deltaTime)
        {
            this.transform.position += _heading * deltaTime;
        }
    }
}
