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

        private IEnumerator Coroutine_SpawnAsteroids(int count)
        {
            yield return null;

            while (_asteroidContainer.childCount < count)
            {
                Object.Instantiate(
                    _asteroidPrefab,
                    _GetRandomWorldPositionInRect(_asteroidContainer),
                    Quaternion.identity,
                    _asteroidContainer);

                if (_delayBetweenSpawns > 0f) yield return new WaitForSeconds(_delayBetweenSpawns);
            }
        }

        private void HandleAsteroidCountChanged(UnityEngine.Object _, int count)
        {
            if (_asteroidContainer.childCount >= count) return;

            this.StartCoroutine(Coroutine_SpawnAsteroids(count));
        }

        private void HandleDayChanged(UnityEngine.Object _, int day)
        {
            if (_asteroidContainer.childCount == 0) return;

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
            while (_asteroidContainer.childCount < _locationData.AsteroidCount)
            {
                Object.Instantiate(
                    _asteroidPrefab,
                    _GetRandomWorldPositionInRect(_asteroidContainer),
                    Quaternion.identity,
                    _asteroidContainer);
            }
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
