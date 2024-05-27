using System;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Fx
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleFx : EntityBase<FxManager>, IParticleFx
    {
        [NonSerialized] private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
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
            if (!_particleSystem.main.loop)
            {
                UnityEngine.Object.Destroy(this.gameObject, _particleSystem.main.duration);
            }
        }
    }
}
