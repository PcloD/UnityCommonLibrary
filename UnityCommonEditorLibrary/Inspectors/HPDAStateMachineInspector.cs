using System;
using UnityCommonLibrary.FSM;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEditorInternal;
using UnityCommonLibrary.Utilities;

namespace UnityCommonEditorLibrary.Inspectors {
	/// <summary>
	/// Custom editor for <see cref="HPDAStateMachine"/>.
	/// Uses reorderable list to make state management easier.
	/// </summary>
	[CustomEditor(typeof(HPDAStateMachine))]
	public class HPDAStateMachineInspector : Editor {
		#region Fields
		/// <summary>
		/// The strongly typed inspected object.
		/// </summary>
		private HPDAStateMachine machine;
		/// <summary>
		/// The ID field of the <see cref="machine"/>
		/// </summary>
		private SerializedProperty id;
		private ReorderableList stateList;

		/// <summary>
		/// Keeps track of the total scroll area position
		/// </summary>
		private Vector2 scroll;
		/// <summary>
		/// Animates the info foldout
		/// </summary>
		private AnimBool showInfo;
		/// <summary>
		/// Animates the state buttons foldout
		/// </summary>
		private AnimBool showStateButtons;
		/// <summary>
		/// A bold label style for foldouts
		/// </summary>
		private GUIStyle boldFoldout;
		/// <summary>
		/// Keeps track of if the inspected object is a prefab or not.
		/// </summary>
		private PrefabType type;
		#endregion

		/// <summary>
		/// Determines if the Play Mode controls should be shown.
		/// </summary>
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

				// Create and configure reorderable list
				stateList = new ReorderableList(serializedObject, states);
				stateList.drawHeaderCallback += DrawStateListHeader;
				stateList.onAddDropdownCallback += StateListDropdown;
				stateList.onSelectCallback += StateListSelect;
				stateList.onRemoveCallback += RemoveStateFromList;
				stateList.drawElementCallback += DrawStateListElement;
			}
			else {
				// Create and configure animbools
				showInfo = new AnimBool(true);
				showInfo.valueChanged.AddListener(Repaint);
				showStateButtons = new AnimBool(true);
				showStateButtons.valueChanged.AddListener(Repaint);

				boldFoldout = EditorStyles.foldout;
				boldFoldout.fontStyle = FontStyle.Bold;
			}
		}

		/// <summary>
		/// The first call to DoRemove just clears the value,
		/// we want to call it twice to remove it completely immediately.
		/// </summary>
		private void RemoveStateFromList(ReorderableList list) {
			ReorderableList.defaultBehaviours.DoRemoveButton(list);
			ReorderableList.defaultBehaviours.DoRemoveButton(list);
		}

		/// <summary>
		/// Draws the title for the reorderable lsit
		/// </summary>
		private void DrawStateListHeader(Rect rect) {
			EditorGUI.LabelField(rect, "States");
		}

		/// <summary>
		/// Selects the selected object in the editor hierarchy
		/// </summary>
		private void StateListSelect(ReorderableList list) {
			EditorGUIUtility.PingObject(stateList.serializedProperty.GetArrayElementAtIndex(list.index).objectReferenceValue);
		}

		/// <summary>
		/// Draws a custom dropdown for the add menu.
		/// </summary>
		private void StateListDropdown(Rect buttonRect, ReorderableList list) {
			var menu = new GenericMenu();
			// Get all states that are on or are children of this GameObject
			var children = machine.GetComponentsInChildren<HPDAState>();

			foreach(var c in children) {
				// Skip any states of a type present in the list already
				if(ListContainsStateType(c.GetType())) {
					continue;
				}
				// Add a new menu item that when clicked adds the type to the list
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

		/// <summary>
		///  Checks if any of the states in the list are of type <paramref name="type"/>
		/// </summary>
		/// <param name="type">The type to check in the list.</param>
		/// <returns>True if the type exists, false otherwise</returns>
		private bool ListContainsStateType(Type type) {
			for(int i = 0; i < stateList.serializedProperty.arraySize; i++) {
				var obj = stateList.serializedProperty.GetArrayElementAtIndex(i);
				if(obj.objectReferenceValue.GetType() == type) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Draws a single element of the list.
		/// </summary>
		private void DrawStateListElement(Rect rect, int index, bool isActive, bool isFocused) {
			var element = stateList.serializedProperty.GetArrayElementAtIndex(index);
			// if the object or its reference are null, skip
			if(element == null || element.objectReferenceValue == null) {
				return;
			}
			// small adjustment to position
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

			// Draw ID field
			var oldValue = id.stringValue;
			EditorGUILayout.PropertyField(id, new GUIContent("ID"));
			id.stringValue = StringUtility.IsNullOrWhitespace(id.stringValue) ?
							 oldValue :
							 id.stringValue;

			// Add space then draw list
			EditorGUILayout.Separator();
			stateList.DoLayoutList();

			// Apply any changes
			serializedObject.ApplyModifiedProperties();
		}

		private void DrawPlayControls() {
			EditorGUILayout.LabelField("Play Controls", EditorStyles.boldLabel);

			// Disable Rewind button if there is no history
			GUI.enabled = machine.historyCount > 0;
			if(GUILayout.Button("Rewind History")) {
				machine.Rewind();
			}
			GUI.enabled = true;

			EditorGUILayout.Separator();

			// Draw state buttons
			// Set animbool target and draw dropdown for states
			showStateButtons.target = EditorGUILayout.Foldout(showStateButtons.target, "States", boldFoldout);
			if(EditorGUILayout.BeginFadeGroup(showStateButtons.faded)) {
				// Scroll view needs minimum height to look decent
				var minHeight = GUILayout.MinHeight(30f * machine.readonlyStates.Count);
				scroll = EditorGUILayout.BeginScrollView(scroll, minHeight);
				foreach(var s in machine.readonlyStates) {
					// Disable the state button if we're in that state
					// so we cant attempt to transition to it
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

			// Draw info box
			// Set anim bool target and draw info dropdown
			showInfo.target = EditorGUILayout.Foldout(showInfo.target, "Info", boldFoldout);
			if(EditorGUILayout.BeginFadeGroup(showInfo.faded)) {
				EditorGUILayout.HelpBox(machine.ToString(), MessageType.Info);
			}
			EditorGUILayout.EndFadeGroup();

			// Fire repaint so the state of the machine is reflected
			// by this editor.
			Repaint();
		}
	}
}
