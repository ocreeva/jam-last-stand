using System.Collections;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Enemies
{
    public class AsteroidSpawner : TraitBase<EnemyManager>, IEnemySpawner
    {
        [Header("Configuration")]
        [SerializeField, Range(0, 180)] private int _startDelay;
        [SerializeField, Range(0, 180)] private int _minDelay;
        [SerializeField, Range(0f, 60f)] private float _delayVariance;

        [Header("Prefabs")]
        [SerializeField] private Enemy _enemyPrefab;

        private IEnumerator Start()
        {
            _manager.Register(this);

            var location = Omnibus.Planet.Target.Location;
            var locationData = Omnibus.Planet.GetLocationData(location);
            var asteroidCount = locationData.AsteroidCount;

            float nextDelay = _startDelay;
            while (asteroidCount > 0)
            {
                yield return new WaitForSeconds(nextDelay);

                this.SpawnEnemy();

                asteroidCount--;
                nextDelay = _minDelay + UnityEngine.Random.value * _delayVariance;
            }

            _manager.Deregister(this);
        }

        private void SpawnEnemy()
        {
            var maximumDistance = Omnibus.Bounds.MaximumDistance;
            var x = UnityEngine.Random.value * maximumDistance * 2 - maximumDistance;
            UnityEngine.Object.Instantiate(_enemyPrefab, new Vector2(x, maximumDistance), Quaternion.identity, _manager.Container);
        }
    }
}
