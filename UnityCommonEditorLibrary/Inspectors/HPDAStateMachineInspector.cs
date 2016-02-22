using System;
using UnityCommonLibrary.FSM;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEditorInternal;
using UnityCommonLibrary.Utilities;

namespace UnityCommonEditorLibrary.Inspectors {
    [CustomEditor(typeof(HPDAStateMachine))]
    public class HPDAStateMachineInspector : Editor {
        private HPDAStateMachine machine;
        private SerializedProperty id;
        private ReorderableList stateList;

        private Vector2 scroll;
        private AnimBool showInfo;
        private AnimBool showStateButtons;
        private GUIStyle boldFoldout;
        private PrefabType type;

        private bool isShowingPlayControls {
            get {
                return Application.isPlaying && type != PrefabType.Prefab;
            }
        }

        private void OnEnable() {
            machine = target as HPDAStateMachine;
            type = PrefabUtility.GetPrefabType(machine.gameObject);

            if(!isShowingPlayControls) {
                id = serializedObject.FindProperty("_id");
                var states = serializedObject.FindProperty("states");

                stateList = new ReorderableList(serializedObject, states);

                stateList.drawHeaderCallback += DrawStateListHeader;
                stateList.onAddDropdownCallback += StateListDropdown;
                stateList.onSelectCallback += StateListSelect;
                stateList.onRemoveCallback += RemoveStateFromList;
                stateList.drawElementCallback += DrawStateListElement;
            }
            else {
                showInfo = new AnimBool(true);
                showInfo.valueChanged.AddListener(Repaint);
                showStateButtons = new AnimBool(true);
                showStateButtons.valueChanged.AddListener(Repaint);

                boldFoldout = EditorStyles.foldout;
                boldFoldout.fontStyle = FontStyle.Bold;
            }
        }

        private void RemoveStateFromList(ReorderableList list) {
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
        }

        private void DrawStateListHeader(Rect rect) {
            EditorGUI.LabelField(rect, "States");
        }

        private void StateListSelect(ReorderableList list) {
            EditorGUIUtility.PingObject(stateList.serializedProperty.GetArrayElementAtIndex(list.index).objectReferenceValue);
        }

        private void StateListDropdown(Rect buttonRect, ReorderableList list) {
            var menu = new GenericMenu();
            var children = machine.GetComponentsInChildren<HPDAState>();
            foreach(var c in children) {
                if(ListContainsStateType(c.GetType())) {
                    continue;
                }
                menu.AddItem(new GUIContent(c.GetType().Name), false, () => {
                    var index = list.serializedProperty.arraySize;
                    list.serializedProperty.arraySize++;
                    list.index = index;
                    var obj = list.serializedProperty.GetArrayElementAtIndex(index);
                    obj.objectReferenceValue = c;
                    serializedObject.ApplyModifiedProperties();
                });
            }
            menu.ShowAsContext();
        }

        private bool ListContainsStateType(Type type) {
            for(int i = 0; i < stateList.serializedProperty.arraySize; i++) {
                var obj = stateList.serializedProperty.GetArrayElementAtIndex(i);
                if(obj.objectReferenceValue.GetType() == type) {
                    return true;
                }
            }
            return false;
        }

        private void DrawStateListElement(Rect rect, int index, bool isActive, bool isFocused) {
            var element = stateList.serializedProperty.GetArrayElementAtIndex(index);
            if(element == null || element.objectReferenceValue == null) {
                return;
            }
            rect.y += 2f;
            EditorGUI.LabelField(rect, element.objectReferenceValue.GetType().Name);
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.Separator();
            if(isShowingPlayControls) {
                DrawPlayControls();
            }
            else {
                DrawEditControls();
            }
        }

        private void DrawEditControls() {
            EditorGUILayout.LabelField("Edit Controls", EditorStyles.boldLabel);
            serializedObject.Update();

            var oldValue = id.stringValue;
            EditorGUILayout.PropertyField(id, new GUIContent("ID"));
            id.stringValue = StringUtility.IsNullOrWhitespace(id.stringValue) ?
                             oldValue :
                             id.stringValue;

            EditorGUILayout.Separator();
            stateList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPlayControls() {
            EditorGUILayout.LabelField("Play Controls", EditorStyles.boldLabel);
            GUI.enabled = machine.historyCount > 0;
            if(GUILayout.Button("Rewind History")) {
                machine.Rewind();
            }
            GUI.enabled = true;

            // State buttons
            EditorGUILayout.Separator();
            showStateButtons.target = EditorGUILayout.Foldout(showStateButtons.target, "States", boldFoldout);
            if(EditorGUILayout.BeginFadeGroup(showStateButtons.faded)) {
                var minHeight = GUILayout.MinHeight(30f * machine.readonlyStates.Count);
                scroll = EditorGUILayout.BeginScrollView(scroll, minHeight);
                foreach(var s in machine.readonlyStates) {
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

            // Info box
            showInfo.target = EditorGUILayout.Foldout(showInfo.target, "Info", boldFoldout);
            if(EditorGUILayout.BeginFadeGroup(showInfo.faded)) {
                EditorGUILayout.HelpBox(machine.ToString(), MessageType.Info);
            }
            EditorGUILayout.EndFadeGroup();
            Repaint();
        }
    }
}
