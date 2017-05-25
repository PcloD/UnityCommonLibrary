using System;
using UnityEngine;

namespace UnityCommonLibrary.Attributes
{
    /// <summary>
    ///     Makes this field not editable in the inspector, but visible.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyAttribute : PropertyAttribute { }
}