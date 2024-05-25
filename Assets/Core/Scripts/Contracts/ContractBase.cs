using System.Collections.Generic;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;

namespace Moyba.Contracts
{
    public abstract class ContractBase : MonoBehaviour
    {
        protected void _Set<T>(
            T value,
            ref T field,
            ValueEventHandler<T> changing = null,
            ValueEventHandler<T> changed = null,
            bool includeIdempotent = false)
        {
            if (!includeIdempotent && EqualityComparer<T>.Default.Equals(value, field)) return;

            changing?.Invoke(this, field);

            field = value;

            changed?.Invoke(this, field);
        }

        protected void _Set(
            bool value,
            ref bool field,
            SimpleEventHandler onFalse = null,
            SimpleEventHandler onTrue = null,
            bool includeIdempotent = false)
        {
            if (!includeIdempotent && value == field) return;

            field = value;

            (value ? onTrue : onFalse)?.Invoke(this);
        }

#if UNITY_EDITOR
        protected static T _LoadOmnibusAsset<T>()
            where T : class
        {
            var name = _ContractUtility.GetFeatureName<T>();
            var asset = _LoadOmnibusAsset<T>(name);
            if (asset != null) return asset;

            // try pluralizing the name, for collection entities
            name += "s";
            return _LoadOmnibusAsset<T>(name);
        }

        private static T _LoadOmnibusAsset<T>(string name)
            where T : class
            => _LoadAssetAtPath<T>("Assets", name, $"Omnibus.{name}.asset");

        private static T _LoadAssetAtPath<T>(params string[] pathSegments)
            where T : class
        {
            var path = Path.Combine(pathSegments);
            return AssetDatabase.LoadMainAssetAtPath(path) as T;
        }
#endif
    }
}
