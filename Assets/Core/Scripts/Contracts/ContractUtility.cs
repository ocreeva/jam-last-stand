using System.Collections.Generic;

namespace Moyba.Contracts
{
    internal static class _ContractUtility
    {
        private static readonly string[] _CommonSuffixes = { "Manager" };

        public static string GetFeatureName<T>()
        {
            var type = typeof(T);
            var name = type.Name;

            if (type.IsInterface && name.StartsWith('I')) name = name[1..];

            return _ContractUtility.GetFeatureName(name);
        }

        public static string GetFeatureName(string name)
        {
            foreach (var suffix in _CommonSuffixes)
            {
                if (name.EndsWith(suffix)) name = name[..^suffix.Length];
            }

            return name;
        }

        public static void Set<T>(
            UnityEngine.Object source,
            T value,
            ref T field,
            ValueEventHandler<T> changing,
            ValueEventHandler<T> changed,
            bool includeIdempotent)
        {
            if (!includeIdempotent && EqualityComparer<T>.Default.Equals(value, field)) return;

            changing?.Invoke(source, field);

            field = value;

            changed?.Invoke(source, field);
        }

        public static void Set(
            UnityEngine.Object source,
            bool value,
            ref bool field,
            SimpleEventHandler onFalse,
            SimpleEventHandler onTrue,
            bool includeIdempotent)
        {
            if (!includeIdempotent && value == field) return;

            field = value;

            (value ? onTrue : onFalse)?.Invoke(source);
        }
    }
}
