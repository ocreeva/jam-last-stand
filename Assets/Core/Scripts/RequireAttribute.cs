using System;
using UnityEngine;

namespace Moyba
{
    public class RequireAttribute : PropertyAttribute
    {
        public RequireAttribute(Type type)
            => this.Type = type;

        public Type Type { get; }
    }
}
