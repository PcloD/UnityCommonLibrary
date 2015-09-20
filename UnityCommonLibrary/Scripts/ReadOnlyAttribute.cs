using System;
using UnityEngine;

namespace UnityCommonLibrary {
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    /// <summary>
    /// Makes this field not editable in the inspector, but visible.
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute { }
}