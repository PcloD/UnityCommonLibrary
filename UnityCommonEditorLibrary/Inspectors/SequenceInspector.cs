using UnityCommonLibrary;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace UnityCommonEditorLibrary.Inspectors
{
    [CustomEditor(typeof(Sequence), true)]
    public class SequenceInspector : Editor
    {
        private SerializedProperty _destroyOnComplete;
        private SerializedProperty _executeOnStart;
        private AnimBool _isFoldout;
        private SerializedProperty _loop;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            EditorGUI.indentLevel = 1;
            _isFoldout.target =
                EditorGUILayout.Foldout(_isFoldout.target, "Sequence Configuration");
            if (EditorGUILayout.BeginFadeGroup(_isFoldout.faded))
            {
                EditorGUI.indentLevel++;
                _executeOnStart.boolValue =
                    EditorGUILayout.ToggleLeft(_executeOnStart.displayName,
                        _executeOnStart.boolValue);
                _destroyOnComplete.boolValue =
                    EditorGUILayout.ToggleLeft(_destroyOnComplete.displayName,
                        _destroyOnComplete.boolValue);
                _loop.boolValue =
                    EditorGUILayout.ToggleLeft(_loop.displayName, _loop.boolValue);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            _executeOnStart = serializedObject.FindProperty("executeOnStart");
            _loop = serializedObject.FindProperty("loop");
            _destroyOnComplete = serializedObject.FindProperty("destroyOnComplete");

            _isFoldout = new AnimBool(false);
            _isFoldout.valueChanged.AddListener(Repaint);
        }
    }
}