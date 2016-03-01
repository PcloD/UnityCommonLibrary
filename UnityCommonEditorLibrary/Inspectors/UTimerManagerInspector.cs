using UnityEditor;
using UnityCommonLibrary.Time;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors {
    [CustomEditor(typeof(UTimerManager))]
    public class UTimerManagerInspector : Editor {
        private UTimerManager manager;
        private Vector2 scroll;

        private void OnEnable() {
            manager = target as UTimerManager;
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach(var t in UTimer.allReadonly) {
                EditorGUILayout.LabelField(t.ToString());
            }
            EditorGUILayout.EndScrollView();
            Repaint();
        }
    }
}
