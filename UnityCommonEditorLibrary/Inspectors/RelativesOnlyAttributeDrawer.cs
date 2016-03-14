using UnityCommonLibrary.Attributes;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors {
	[CustomPropertyDrawer(typeof(RelativesOnlyAttribute))]
	public class RelativesOnlyAttributeDrawer : PropertyDrawer {
		private const string MSG = "This object field only allows assignments from the following relatives:\n\n{0}\n\nThe current value will be unassigned.";

		private RelativesOnlyAttribute target;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.PropertyField(position, property, label);

			var component = property.objectReferenceValue as Component;
			if(component != null) {
				target = attribute as RelativesOnlyAttribute;
				var isValid = ProcessRules(property, component.gameObject);
				if(!isValid) {
					EditorUtility.DisplayDialog("Invalid Reference", string.Format(MSG, target.validRelatives), "OK");
					property.objectReferenceValue = null;
				}
			}
		}

		private bool ProcessRules(SerializedProperty property, GameObject obj) {
			var propertyGameObject = (property.serializedObject.targetObject as Component).gameObject;

			// First check hard-set rules
			if(target.IsOnlyRuleSet(ValidRelatives.SameGameObject)) {
				return obj == propertyGameObject;
			}
			if(target.IsOnlyRuleSet(ValidRelatives.Children)) {
				return obj.transform.IsChildOf(propertyGameObject.transform) && obj != propertyGameObject;
			}
			if(target.IsOnlyRuleSet(ValidRelatives.Parents)) {
				return propertyGameObject.transform.IsChildOf(obj.transform) && obj != propertyGameObject;
			}

			// Then check multi-applicable rules
			if(target.IsRuleSet(ValidRelatives.SameGameObject) && obj == propertyGameObject) {
				return true;
			}
			if(target.IsRuleSet(ValidRelatives.Children) && obj.transform.IsChildOf(propertyGameObject.transform) && obj != propertyGameObject) {
				return true;
			}
			if(target.IsRuleSet(ValidRelatives.Parents) && propertyGameObject.transform.IsChildOf(obj.transform) && obj != propertyGameObject) {
				return true;
			}
			return false;
		}
	}
}
