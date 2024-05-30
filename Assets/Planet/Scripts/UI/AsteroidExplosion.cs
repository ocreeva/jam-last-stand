using System.Collections;
using UnityEngine;

namespace Moyba.Planet.UI
{
    public class AsteroidExplosion : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField, Range(float.Epsilon, 10f)] private float _lifetime = 1f;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_lifetime);

            UnityEngine.Object.Destroy(this.gameObject);
        }
    }
}
