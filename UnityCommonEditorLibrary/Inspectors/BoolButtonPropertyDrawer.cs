using UnityCommonLibrary.Attributes;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors
{
    [CustomPropertyDrawer(typeof(BoolButton))]
    public class BoolButtonPropertyDrawer : PropertyDrawer
    {
        /// <inheritdoc />
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.boolValue = GUI.Button(position, label);
        }
    }
}
