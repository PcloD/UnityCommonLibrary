using System.Linq;
using UnityCommonLibrary.FSM;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors {
    [CustomPropertyDrawer(typeof(HPDAStateMachine))]
    public class HPDAStateMachinePropertyDrawer : PropertyDrawer {
        private GameObject objReference;
        private float space = 0f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label) * 3f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            var labelPosition = new Rect(position);
            EditorGUI.LabelField(labelPosition, label, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            position.height /= 3f;
            position.y += position.height;

            if(objReference == null) {
                objReference = property.objectReferenceValue != null ?
                               (property.objectReferenceValue as HPDAStateMachine).gameObject :
                                null;
            }
            var rect = EditorGUI.PrefixLabel(position, new GUIContent("Container"));
            objReference = EditorGUI.ObjectField(rect, GUIContent.none, objReference, typeof(GameObject), false) as GameObject;


            if(objReference != null) {
                var scriptGO = (property.serializedObject.targetObject as Component).gameObject;
                if(!scriptGO.transform.IsChildOf(objReference.transform)) {
                    objReference = null;
                }
                else {
                    var allMachines = objReference.GetComponents<HPDAStateMachine>();
                    if(allMachines.Length > 0) {
                        var index = property.objectReferenceValue == null ?
                                    0 :
                                    ArrayUtility.IndexOf(allMachines, property.objectReferenceValue);

                        position.y += position.height;
                        rect = EditorGUI.PrefixLabel(position, new GUIContent("Machine"));
                        index = EditorGUI.Popup(rect, index, allMachines.Select(m => m.id).ToArray());
                        property.objectReferenceValue = allMachines[index];
                    }
                }
            }

            if(objReference == null) {
                property.objectReferenceValue = null;
            }
            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

    }
}
