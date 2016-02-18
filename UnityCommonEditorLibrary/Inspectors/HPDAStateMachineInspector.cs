using UnityCommonLibrary.FSM;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors {
    [CustomEditor(typeof(HPDAStateMachine))]
    public class HPDAStateMachineInspector : Editor {
        private HPDAStateMachine machine;
        private Vector2 scroll;
        private AnimBool showInfo;
        private AnimBool showStateButtons;
        private GUIStyle boldFoldout;

        private void OnEnable() {
            if(machine == null) {
                machine = target as HPDAStateMachine;
            }
            showInfo = new AnimBool(true);
            showInfo.valueChanged.AddListener(Repaint);
            showStateButtons = new AnimBool(true);
            showStateButtons.valueChanged.AddListener(Repaint);

            boldFoldout = EditorStyles.foldout;
            boldFoldout.fontStyle = FontStyle.Bold;
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            var type = PrefabUtility.GetPrefabType(machine.gameObject);
            if(Application.isPlaying && type != PrefabType.Prefab) {
                DrawStateButtons();
                showInfo.target = EditorGUILayout.Foldout(showInfo.target, "Info", boldFoldout);
                if(EditorGUILayout.BeginFadeGroup(showInfo.faded)) {
                    EditorGUILayout.HelpBox(machine.ToString(), MessageType.Info);
                }
                EditorGUILayout.EndFadeGroup();
                Repaint();
            }
        }

        private void DrawStateButtons() {
            // General controls
            EditorGUILayout.LabelField("Controls", EditorStyles.boldLabel);
            GUI.enabled = machine.historyCount > 0;
            if(GUILayout.Button("Rewind History")) {
                machine.Rewind();
            }
            GUI.enabled = true;

            // State buttons
            EditorGUILayout.Separator();
            showStateButtons.target = EditorGUILayout.Foldout(showStateButtons.target, "States", boldFoldout);
            if(EditorGUILayout.BeginFadeGroup(showStateButtons.faded)) {
                var minHeight = GUILayout.MinHeight(30f * machine.allStates.Count);
                scroll = EditorGUILayout.BeginScrollView(scroll, minHeight);
                foreach(var s in machine.allStates) {
                    if(machine.currentState == s) {
                        GUI.enabled = false;
                    }
                    if(GUILayout.Button(s.GetType().Name)) {
                        machine.SwitchState(s);
                    }
                    GUI.enabled = true;
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndFadeGroup();
        }
    }
}
