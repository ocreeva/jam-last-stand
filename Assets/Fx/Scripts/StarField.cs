using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Fx
{
    public class StarField : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField, Range(1f, 20f)] private float _radius = 1f;
        [SerializeField, Range(0f, 1f)] private float _scale = 0f;
        [SerializeField, Range(-1024f, 0f)] private float _xMin = 0f;
        [SerializeField, Range(0f, 1024f)] private float _xMax = 0f;
        [SerializeField, Range(-1024f, 0f)] private float _yMin = 0f;
        [SerializeField, Range(0f, 1024f)] private float _yMax = 0f;

        [Header("Prefabs")]
        [SerializeField] private GameObject _starPrefab;

        private void Start()
        {
            var xStep = _radius * Mathf.Sqrt(3f);
            var yStep = _radius * 2f;

            var yStart = 0f;
            for (var x = 0f; x < _xMax; x += xStep)
            {
                for (var y = yStart; y < _yMax; y += yStep)
                {
                    this.Start_InstantiateStar(x, y);
                }

                for (var y = yStart - yStep; y > _yMin; y -= yStep)
                {
                    this.Start_InstantiateStar(x, y);
                }

                yStart = yStart < float.Epsilon ? _radius : 0f;
            }

            yStart = _radius;
            for (var x = -xStep; x > _xMin; x -= xStep)
            {
                for (var y = yStart; y < _yMax; y += yStep)
                {
                    this.Start_InstantiateStar(x, y);
                }

                for (var y = yStart - yStep; y > _yMin; y -= yStep)
                {
                    this.Start_InstantiateStar(x, y);
                }

                yStart = yStart < float.Epsilon ? _radius : 0f;
            }
        }

        private void Start_InstantiateStar(float x, float y)
        {
            var position = new Vector2(x, y) + Random.insideUnitCircle * _radius;
            UnityEngine.Object.Instantiate(_starPrefab, position, Quaternion.identity, this.transform);
        }

        private void Update()
        {
            if (_scale < float.Epsilon) return;

            var shipPosition = Omnibus.Ship.Position;
            this.transform.position = new Vector3(
                shipPosition.x * _scale,
                shipPosition.y * _scale,
                this.transform.position.z);
        }
    }
}
