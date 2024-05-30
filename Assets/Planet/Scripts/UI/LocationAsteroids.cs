using System.Collections;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Planet.UI
{
    public class LocationAsteroids : MonoBehaviour
    {
        [SerializeField] private LocationData _locationData;

        [Header("Components")]
        [SerializeField] private RectTransform _asteroidContainer;
        [SerializeField] private RectTransform _targetContainer;

        [Header("Configuration")]
        [SerializeField, Range(float.Epsilon, 10f)] private float _delayBetweenLaunches = 0f;
        [SerializeField, Range(float.Epsilon, 10f)] private float _delayBetweenSpawns = 0f;

        [Header("Prefabs")]
        [SerializeField] private Asteroid _asteroidPrefab;

        private IEnumerator Coroutine_LaunchAsteroids()
        {
            for (var index = _asteroidContainer.childCount - 1; index >= 0; index--)
            {
                var asteroid = _asteroidContainer.GetChild(index).GetComponent<Asteroid>();
                if (asteroid == null) continue;

                asteroid.BombardPlanet(_locationData, _targetContainer);

                if (_delayBetweenLaunches > 0f) yield return new WaitForSeconds(_delayBetweenLaunches);
            }
        }

        private IEnumerator Coroutine_SpawnAsteroids(int count, float delayBetweenSpawns)
        {
            if (delayBetweenSpawns > 0f) yield return null;

            while (_asteroidContainer.childCount < count)
            {
                Object.Instantiate(
                    _asteroidPrefab,
                    _GetRandomWorldPositionInRect(_asteroidContainer),
                    Quaternion.identity,
                    _asteroidContainer);

                if (delayBetweenSpawns > 0f) yield return new WaitForSeconds(delayBetweenSpawns);
            }
        }

        private void HandleAsteroidCountChanged(UnityEngine.Object _, int count)
        {
            this.StartCoroutine(Coroutine_SpawnAsteroids(count, _delayBetweenSpawns));
        }

        private void HandleDayChanged(UnityEngine.Object _, int day)
        {
            this.StartCoroutine(Coroutine_LaunchAsteroids());
        }

        private void OnDisable()
        {
            Omnibus.Planet.Time.OnDayChanged -= this.HandleDayChanged;

            _locationData.OnAsteroidCountChanged -= this.HandleAsteroidCountChanged;
        }

        private void OnEnable()
        {
            Omnibus.Planet.Time.OnDayChanged += this.HandleDayChanged;

            _locationData.OnAsteroidCountChanged += this.HandleAsteroidCountChanged;
        }

        private void Start()
        {
            if (_locationData.AsteroidCount > 0) this.StartCoroutine(Coroutine_SpawnAsteroids(_locationData.AsteroidCount, 0f));
        }

        private static Vector3 _GetRandomWorldPositionInRect(RectTransform rectTransform)
        {
            var rect = rectTransform.rect;
            var randomX = Random.Range(rect.xMin, rect.xMax);
            var randomY = Random.Range(rect.yMin, rect.yMax);
            var localPosition = new Vector3(randomX, randomY, 0);
            return rectTransform.TransformPoint(localPosition);
        }
    }
}
