using UnityCommonLibrary;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors
{
    [CustomEditor(typeof(SimpleSpriteAnimation))]
    public class SimpleSpriteAnimationInspector : Editor
    {
        private SerializedProperty _fps;
        private ReorderableList _framesList;
        private SerializedProperty _loop;
        private SerializedProperty _timeMode;

        private bool _useSimpleUi;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_timeMode);
            EditorGUILayout.PropertyField(_fps);
            EditorGUILayout.PropertyField(_loop);
            EditorGUILayout.Separator();
            _useSimpleUi = EditorGUILayout.ToggleLeft("Use Simple UI", _useSimpleUi);
            if (_useSimpleUi)
            {
                EditorGUILayout.PropertyField(_framesList.serializedProperty, true);
            }
            else
            {
                _framesList.DoLayoutList();
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void AddNewElement(ReorderableList list)
        {
            ReorderableList.defaultBehaviours.DoAddButton(list);
            var last =
                list.serializedProperty.GetArrayElementAtIndex(
                    list.serializedProperty.arraySize - 1);
            last.objectReferenceValue = null;
        }

        private void DrawFrameElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var value = _framesList.serializedProperty.GetArrayElementAtIndex(index);
            var sprite = value.objectReferenceValue as Sprite;
            EditorGUI.ObjectField(rect, value, GUIContent.none);
        }

        private void OnEnable()
        {
            _timeMode = serializedObject.FindProperty("timeMode");
            _fps = serializedObject.FindProperty("fps");
            _loop = serializedObject.FindProperty("loop");

            _framesList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("frames"));
            _framesList.drawElementCallback += DrawFrameElement;
            _framesList.onAddCallback += AddNewElement;
            _framesList.drawHeaderCallback += rect =>
            {
                EditorGUI.LabelField(rect, "Animation Frames");
            };
        }
    }
}