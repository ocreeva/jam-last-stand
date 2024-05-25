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
    }
}
