using System.Collections;
using UnityEngine;

namespace Moyba.Planet.UI
{
    public class Asteroid : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField, Range(0f, 1f)] private float _impactDamage = .1f;
        [SerializeField, Range(0f, 1f)] private float _impactDamageVariance = 0f;
        [SerializeField, Range(float.Epsilon, 10f)] private float _impactTime = 1f;
        [SerializeField, Range(0f, 10f)] private float _impactTimeVariance = 0f;

        [Header("Prefabs")]
        [SerializeField] private Object _explosionFxPrefab;

        private Coroutine _bombardCoroutine;

        internal void BombardPlanet(LocationData locationData, RectTransform targetContainer)
        {
            if (_bombardCoroutine != null) return;

            _bombardCoroutine = this.StartCoroutine(Coroutine_BombardPlanet(locationData, targetContainer));
        }

        private IEnumerator Coroutine_BombardPlanet(LocationData locationData, RectTransform targetContainer)
        {
            var targetPosition = _GetRandomWorldPositionInRect(targetContainer);

            var targetTime = _impactTime + _impactTimeVariance * Random.value;
            var heading = (targetPosition - this.transform.position) / targetTime;

            while (targetTime > 0f)
            {
                yield return null;

                var deltaTime = Time.deltaTime;
                targetTime -= deltaTime;
                this.transform.position += heading * deltaTime;
            }

            var damage = _impactDamage + Random.value * _impactDamageVariance;
            locationData.ApplyDamage(damage);

            UnityEngine.Object.Instantiate(_explosionFxPrefab, this.transform.position, Quaternion.identity, targetContainer);
            UnityEngine.Object.Destroy(this.gameObject);
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
