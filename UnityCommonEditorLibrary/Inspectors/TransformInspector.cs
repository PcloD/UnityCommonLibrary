using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors
{
    [CanEditMultipleObjects]
    public class TransformInspector : Editor
    {
        private const float FIELD_WIDTH = 212.0f;
        private const float POSITION_MAX = 100000.0f;
        private const bool WIDE_MODE = true;

        private static readonly GUIContent _positionGuiContent =
                new GUIContent(LocalString("Position"),
                    LocalString(
                        "The local position of this Game Object relative to the parent."))
            ;

        private static readonly GUIContent _rotationGuiContent =
            new GUIContent(LocalString("Rotation"),
                LocalString(
                    "The local rotation of this Game Object relative to the parent."));

        private static readonly GUIContent _scaleGuiContent =
            new GUIContent(LocalString("Scale"),
                LocalString("The local scaling of this Game Object relative to the parent."));

        private static readonly string _positionWarningText =
            LocalString("Due to floating-point precision limitations," +
                        " it is recommended to bring the world coordinates" +
                        " of the GameObject within a smaller range.");


        private SerializedProperty _positionProperty;
        private SerializedProperty _rotationProperty;
        private SerializedProperty _scaleProperty;

        private static string LocalString(string text)
        {
            return LocalizationDatabase.GetLocalizedString(text);
        }

        private static bool SameRotation(Quaternion rot1, Quaternion rot2)
        {
            if (rot1.x != rot2.x)
            {
                return false;
            }
            if (rot1.y != rot2.y)
            {
                return false;
            }
            if (rot1.z != rot2.z)
            {
                return false;
            }
            if (rot1.w != rot2.w)
            {
                return false;
            }
            return true;
        }

        private static bool ValidatePosition(Vector3 position)
        {
            if (Mathf.Abs(position.x) > POSITION_MAX)
            {
                return false;
            }
            if (Mathf.Abs(position.y) > POSITION_MAX)
            {
                return false;
            }
            if (Mathf.Abs(position.z) > POSITION_MAX)
            {
                return false;
            }
            return true;
        }

        public void OnEnable()
        {
            _positionProperty = serializedObject.FindProperty("m_LocalPosition");
            _rotationProperty = serializedObject.FindProperty("m_LocalRotation");
            _scaleProperty = serializedObject.FindProperty("m_LocalScale");
        }

        public override void OnInspectorGUI()
        {
            EditorGUIUtility.wideMode = WIDE_MODE;
            // align field to right of inspector
            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - FIELD_WIDTH;

            serializedObject.Update();

            EditorGUILayout.PropertyField(_positionProperty, _positionGuiContent);
            RotationPropertyField(_rotationProperty, _rotationGuiContent);
            EditorGUILayout.PropertyField(_scaleProperty, _scaleGuiContent);

            if (!ValidatePosition(((Transform) target).position))
            {
                EditorGUILayout.HelpBox(_positionWarningText, MessageType.Warning);
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void RotationPropertyField(SerializedProperty rotationProperty,
            GUIContent content)
        {
            var transform = (Transform) targets[0];
            var localRotation = transform.localRotation;
            foreach (var t in targets)
            {
                if (!SameRotation(localRotation, ((Transform) t).localRotation))
                {
                    EditorGUI.showMixedValue = true;
                    break;
                }
            }
            EditorGUI.BeginChangeCheck();
            var eulerAngles =
                EditorGUILayout.Vector3Field(content, localRotation.eulerAngles);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(targets, "Rotation Changed");
                foreach (var obj in targets)
                {
                    var t = (Transform) obj;
                    t.localEulerAngles = eulerAngles;
                }
                rotationProperty.serializedObject.SetIsDifferentCacheDirty();
            }
            EditorGUI.showMixedValue = false;
        }
    }
}