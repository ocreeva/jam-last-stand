using System;
using System.Collections;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Enemies
{
    public class EnemySpawner : TraitBase<EnemyManager>
    {
        [Header("Configuration")]
        [SerializeField, Range(0, 180)] private int _startDelay;
        [SerializeField, Range(0, 180)] private int _minDelay;
        [SerializeField, Range(0f, 60f)] private float _delayVariance;
        [SerializeField, Range(0, 180)] private int _duration;

        [Header("Prefabs")]
        [SerializeField] private Enemy _enemyPrefab;

        private IEnumerator Start()
        {
            float remainingDuration = _duration;

            float nextDelay = _startDelay;
            while (remainingDuration > nextDelay)
            {
                yield return new WaitForSeconds(nextDelay);

                this.SpawnEnemy();

                remainingDuration -= nextDelay;
                nextDelay = _minDelay + UnityEngine.Random.value * _delayVariance;
            }
        }

        private void SpawnEnemy()
        {
            var maximumDistance = Omnibus.Bounds.MaximumDistance;
            var x = UnityEngine.Random.value * maximumDistance * 2 - maximumDistance;
            UnityEngine.Object.Instantiate(_enemyPrefab, new Vector2(x, maximumDistance), Quaternion.identity, _manager.Container);
        }
    }
}
