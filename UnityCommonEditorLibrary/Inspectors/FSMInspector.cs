using UnityCommonLibrary.FSM;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors {
    [CustomEditor(typeof(FiniteStateMachine))]
    public class FSMInspector : Editor {
        private FiniteStateMachine machine;

        private void OnEnable() {
            if(machine == null) {
                machine = target as FiniteStateMachine;
            }
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            if(Application.isPlaying) {
                EditorGUILayout.HelpBox(machine.ToString(), MessageType.Info);
                DrawStateButtons();
                Repaint();
            }
        }

        private void DrawStateButtons() {
            foreach(var s in machine.states) {
                EditorGUILayout.LabelField(s.GetType().Name, EditorStyles.boldLabel);
                if(machine.currentState == s) {
                    GUI.enabled = false;
                }
                if(GUILayout.Button("OutIn")) {
                    machine.SwitchState(s);
                }
                if(GUILayout.Button("Crossfade")) {
                    machine.SwitchState(s, StateSwitch.Method.Crossfade);
                }
                if(GUILayout.Button("Overwrite")) {
                    machine.SwitchState(s, StateSwitch.Method.Overwrite);
                }
                GUI.enabled = true;
                EditorGUILayout.Space();
            }
        }
    }
}
