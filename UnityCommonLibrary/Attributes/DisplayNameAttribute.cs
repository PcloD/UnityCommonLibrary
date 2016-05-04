using System;
using UnityEngine;

namespace UnityCommonLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DisplayNameAttribute : PropertyAttribute {
        public GUIContent label { get; private set; }

        /// <summary>
        /// Makes this field display with a different name in the Inspector
        /// </summary>
        /// <param name="name"></param>
        public DisplayNameAttribute(string name) {
            label = new GUIContent(name);
        }
    }
}