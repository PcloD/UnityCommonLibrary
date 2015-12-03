using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary {
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : PropertyDrawer {

        //public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        //    return EditorPrefs.GetBool("ShowReadOnlyFields") ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
        //}

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }

    }
}