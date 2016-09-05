using System;
using UnityEngine;

namespace UnityCommonLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    /// <summary>
    /// Makes this field not editable in the inspector, but visible.
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute { }
}