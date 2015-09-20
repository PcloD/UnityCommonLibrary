using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary {
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if(!EditorPrefs.HasKey("ShowReadOnlyFields")) {
                EditorPrefs.SetBool("ShowReadOnlyFields", true);
            }
            return EditorPrefs.GetBool("ShowReadOnlyFields") ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if(!EditorPrefs.HasKey("ShowReadOnlyFields")) {
                EditorPrefs.SetBool("ShowReadOnlyFields", true);
            }
            if(EditorPrefs.GetBool("ShowReadOnlyFields")) {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = true;
            }

            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }

    }
}