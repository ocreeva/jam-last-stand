using System;
using UnityEngine;

namespace Moyba.Fx
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Star : MonoBehaviour
    {
        private const float _MaximumHue = 360f;

        [Header("Configuration")]
        [SerializeField, Range(0f, 1f)] private float _maximumSaturation = 0f;
        [SerializeField, Range(0f, 1f)] private float _minimumValue = 1f;

        [NonSerialized] private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _spriteRenderer.color = Color.HSVToRGB(
                UnityEngine.Random.value,
                UnityEngine.Random.value * _maximumSaturation,
                _minimumValue + UnityEngine.Random.value * (1f - _minimumValue));
        }
    }
}
