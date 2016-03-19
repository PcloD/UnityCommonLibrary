using UnityCommonLibrary;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors {
    [CustomEditor(typeof(SimpleSpriteAnimation))]
    public class SimpleSpriteAnimationInspector : Editor {
        private ReorderableList framesList;
        private SerializedProperty loop;
        private SerializedProperty timeMode;
        private SerializedProperty fps;

        private bool useSimpleUI;

        private void OnEnable() {
            timeMode = serializedObject.FindProperty("timeMode");
            fps = serializedObject.FindProperty("fps");
            loop = serializedObject.FindProperty("loop");

            framesList = new ReorderableList(serializedObject, serializedObject.FindProperty("frames"));
            framesList.drawElementCallback += DrawFrameElement;
            framesList.onAddCallback += AddNewElement;
            framesList.drawHeaderCallback += (rect) => {
                EditorGUI.LabelField(rect, "Animation Frames");
            };
        }

        private void AddNewElement(ReorderableList list) {
            ReorderableList.defaultBehaviours.DoAddButton(list);
            var last = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
            last.objectReferenceValue = null;
        }

        private void DrawFrameElement(Rect rect, int index, bool isActive, bool isFocused) {
            var value = framesList.serializedProperty.GetArrayElementAtIndex(index);
            var sprite = value.objectReferenceValue as Sprite;
            EditorGUI.ObjectField(rect, value, GUIContent.none);
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(timeMode);
            EditorGUILayout.PropertyField(fps);
            EditorGUILayout.PropertyField(loop);
            EditorGUILayout.Separator();
            useSimpleUI = EditorGUILayout.ToggleLeft("Use Simple UI", useSimpleUI);
            if(useSimpleUI) {
                EditorGUILayout.PropertyField(framesList.serializedProperty, true);
            }
            else {
                framesList.DoLayoutList();
            }

            if(EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

    }
}
