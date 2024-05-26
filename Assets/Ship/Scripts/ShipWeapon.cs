using System;
using System.Collections;
using Moyba.Contracts;
using Moyba.Projectiles;
using UnityEngine;

namespace Moyba.Ship
{
    public class ShipWeapon : TraitBase<ShipManager>
    {
        [Header("Components")]
        [SerializeField] private Transform _source;

        [Header("Configuration")]
        [SerializeField, Range(float.Epsilon, 10f)] private float _delayBetweenShots = 1f;

        [Header("Prefabs")]
        [SerializeField] private UnityEngine.Object _projectilePrefab;

        [NonSerialized] private Coroutine _fireCoroutine;

        private IEnumerator Coroutine_Fire()
        {
            while (true)
            {
                UnityEngine.Object.Instantiate(_projectilePrefab, _source.position, this.transform.rotation, Omnibus.Projectiles.Container);

                yield return new WaitForSeconds(_delayBetweenShots);
            }
        }

        private void HandleFireStarted(UnityEngine.Object _)
        {
            _fireCoroutine = this.StartCoroutine(Coroutine_Fire());
        }

        private void HandleFireStopped(UnityEngine.Object _)
        {
            this.StopCoroutine(_fireCoroutine);
        }

        private void OnDisable()
        {
            Omnibus.Input.Ship.OnFireStarted -= this.HandleFireStarted;
            Omnibus.Input.Ship.OnFireStopped -= this.HandleFireStopped;
        }

        private void OnEnable()
        {
            Omnibus.Input.Ship.OnFireStarted += this.HandleFireStarted;
            Omnibus.Input.Ship.OnFireStopped += this.HandleFireStopped;
        }
    }
}
