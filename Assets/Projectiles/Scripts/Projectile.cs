using System;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Projectiles
{
    public class Projectile : EntityBase<ProjectileManager>, IProjectile
    {
        [Header("Configuration")]
        [SerializeField, Range(float.Epsilon, 10f)] private float _velocity = 1f;
        [SerializeField, Range(float.Epsilon, 10f)] private float _lifetime = 1f;

        [NonSerialized] private float _elapsedTime;

        private void FixedUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;

            this.Update_Move(deltaTime);
        }

        private void OnDisable()
        {
            _manager.Deregister(this);
        }

        private void OnEnable()
        {
            _manager.Register(this);
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;

            this.Update_Lifetime(deltaTime);
        }

        private void Update_Lifetime(float deltaTime)
        {
            _elapsedTime += deltaTime;
            if (_elapsedTime < _lifetime) return;

            UnityEngine.Object.Destroy(this.gameObject);
        }

        private void Update_Move(float deltaTime)
        {
            this.transform.position += this.transform.up * _velocity * deltaTime;
        }
    }
}
