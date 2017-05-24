using System;
using System.Linq;
using System.Reflection;
using UnityCommonLibrary.Attributes;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors
{
    [CustomPropertyDrawer(typeof(EnumMaskAttribute))]
    public class EnumMaskAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            var targetEnum = PropertyDrawerUtils.GetBaseProperty<Enum>(property);

            EditorGUI.BeginProperty(position, label, property);
            var enumNew = EditorGUI.EnumMaskField(position, label, targetEnum);
            property.intValue = (int) Convert.ChangeType(enumNew, targetEnum.GetType());
            EditorGUI.EndProperty();
        }
    }

    public static class PropertyDrawerUtils
    {
        public static T GetBaseProperty<T>(SerializedProperty prop)
        {
            var target = prop.serializedObject.targetObject;
            var fields = target.GetType()
                               .GetFields(BindingFlags.Instance |
                                          BindingFlags.NonPublic | BindingFlags.Public);
            var found = fields.SingleOrDefault(f => f.Name == prop.name)
                              .GetValue(prop.serializedObject.targetObject);
            return (T) found;
        }
    }
}