using System.Linq;
using UnityCommonLibrary.FSM;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors
{
    /// <summary>
    /// A custom property drawer for when a Object field of type <see cref="HPDAStateMachine"/> is
    /// exposed to the inspector.
    /// </summary>
    [CustomPropertyDrawer(typeof(HPDAStateMachine))]
	public class HPDAStateMachinePropertyDrawer : PropertyDrawer {
		/// <summary>
		/// The GameObject we have selected to then select a <see cref="HPDAStateMachine"/>
		/// </summary>
		private GameObject objReference;

		/// <summary>
		/// The editor needs 3 fields worth of height, so multiply the normal value by three.
		/// </summary>
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return base.GetPropertyHeight(property, label) * 3f;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			// Draw the label at the normal position
			var labelPosition = new Rect(position);
			EditorGUI.LabelField(labelPosition, label, EditorStyles.boldLabel);
			EditorGUI.indentLevel++;

			// Move the rect to the normal y position plus 1/3 of the normal height (one field height)
			position.height /= 3f;
			position.y += position.height;

			// Assign the objReference field somehow
			// If the current property value is set, get its GameObject
			// Otherwise set to null
			if(objReference == null) {
				objReference = property.objectReferenceValue != null ?
							   (property.objectReferenceValue as HPDAStateMachine).gameObject :
								null;
			}

			// Draw the GameObject reference field
			var rect = EditorGUI.PrefixLabel(position, new GUIContent("Container"));
			objReference = EditorGUI.ObjectField(rect, GUIContent.none, objReference, typeof(GameObject), true) as GameObject;


			if(objReference != null) {
				// The GameObject of the inspected script
				var scriptGO = (property.serializedObject.targetObject as Component).gameObject;

				// Get all HPDAStateMachines on the objReference
				var allMachines = objReference.GetComponents<HPDAStateMachine>();
				if(allMachines.Length > 0) {
					// Get current selected index, or 0 if null
					var index = property.objectReferenceValue == null ?
								0 :
								ArrayUtility.IndexOf(allMachines, property.objectReferenceValue);

					// Add another single field's height to the position for the final GUI control
					position.y += position.height;
					// Draw a prefix label and popup to select a machine on this GameObject
					rect = EditorGUI.PrefixLabel(position, new GUIContent("Machine"));
					index = EditorGUI.Popup(rect, index, allMachines.Select(m => m.id).ToArray());

					// Set the selected machine
					property.objectReferenceValue = allMachines[index];
				}
			}

			// Finally, if our GameObject is still null via being unassigned at any point
			// the property should also become unassigned
			if(objReference == null) {
				property.objectReferenceValue = null;
			}
			EditorGUI.indentLevel--;
			EditorGUI.EndProperty();
		}

	}
}
