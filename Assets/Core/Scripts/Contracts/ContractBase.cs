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
            => _ContractUtility.Set<T>(this, value, ref field, changing, changed, includeIdempotent);

        protected void _Set(
            bool value,
            ref bool field,
            SimpleEventHandler onFalse = null,
            SimpleEventHandler onTrue = null,
            bool includeIdempotent = false)
            => _ContractUtility.Set(this, value, ref field, onFalse, onTrue, includeIdempotent);

#if UNITY_EDITOR
        // for nonstandard names
        private static readonly IDictionary<string, string> _NameLookup = new Dictionary<string, string>
        {
            { "Enemy", "Enemies" },
        };

        protected static T _LoadOmnibusAsset<T>()
            where T : class
        {
            var name = _ContractUtility.GetFeatureName<T>();
            if (_NameLookup.ContainsKey(name)) name = _NameLookup[name];

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
