using UnityCommonLibrary;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace UnityCommonEditorLibrary.Inspectors
{
    [CustomEditor(typeof(Sequence), true)]
    public class SequenceInspector : Editor {
        private SerializedProperty executeOnStart;
        private SerializedProperty loop;
        private SerializedProperty destroyOnComplete;
        private AnimBool isFoldout;

        private void OnEnable() {
            executeOnStart = serializedObject.FindProperty("executeOnStart");
            loop = serializedObject.FindProperty("loop");
            destroyOnComplete = serializedObject.FindProperty("destroyOnComplete");

            isFoldout = new AnimBool(false);
            isFoldout.valueChanged.AddListener(Repaint);
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            DrawDefaultInspector();

            EditorGUI.indentLevel = 1;
            isFoldout.target = EditorGUILayout.Foldout(isFoldout.target, "Sequence Configuration");
            if(EditorGUILayout.BeginFadeGroup(isFoldout.faded)) {
                EditorGUI.indentLevel++;
                executeOnStart.boolValue = EditorGUILayout.ToggleLeft(executeOnStart.displayName, executeOnStart.boolValue);
                destroyOnComplete.boolValue = EditorGUILayout.ToggleLeft(destroyOnComplete.displayName, destroyOnComplete.boolValue);
                loop.boolValue = EditorGUILayout.ToggleLeft(loop.displayName, loop.boolValue);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();

            serializedObject.ApplyModifiedProperties();
        }

    }

}