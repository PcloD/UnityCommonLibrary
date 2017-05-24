using System;
using UnityEngine;

namespace UnityCommonLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DisplayNameAttribute : PropertyAttribute
    {
        public GUIContent Label { get; }

        /// <summary>
        ///     Makes this field display with a different name in the Inspector
        /// </summary>
        /// <param name="name"></param>
        public DisplayNameAttribute(string name)
        {
            Label = new GUIContent(name);
        }
    }
}