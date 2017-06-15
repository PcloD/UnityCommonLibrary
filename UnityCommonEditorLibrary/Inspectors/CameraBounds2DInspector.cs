using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors
{
    [CustomEditor(typeof(CameraBounds2D))]
    public class CameraBounds2DInspector : Editor
    {
        private const string BOUNDSMISMATCH_ERR =
                "Camera's frustum CANNOT fit into current bounds, camera's position WILL NOT be altered!"
            ;

        private const string NOCAM_ERR = "There is no camera assigned!";

        private const string NOCOLLIDER_ERR =
            "No collider provided, camera's position WILL NOT be altered!";

        private const string UNBOUNDED_ERR =
            "Set to unbounded in all directions, camera's position WILL NOT be altered!";

        private SerializedProperty _boundedXMax;
        private SerializedProperty _boundedXMin;
        private SerializedProperty _boundedYMax;
        private SerializedProperty _boundedYMin;
        private SerializedProperty _bounds;

        private SerializedProperty _camera;
        private bool _isBoundsNull;
        private CameraBounds2D _obj;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Configuration", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_camera);
            EditorGUILayout.PropertyField(_bounds);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Misc.", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_boundedXMin);
            EditorGUILayout.PropertyField(_boundedXMax);
            EditorGUILayout.PropertyField(_boundedYMin);
            EditorGUILayout.PropertyField(_boundedYMax);
            EditorGUILayout.Space();

            _isBoundsNull = Equals(_camera.objectReferenceValue, null);
            //Show Errors
            if (_isBoundsNull)
            {
                GUI.color = Color.red;
                EditorGUILayout.LabelField(NOCAM_ERR, EditorStyles.helpBox);
            }
            else
            {
                if (Equals(_bounds.objectReferenceValue, null))
                {
                    GUI.color = Color.red;
                    EditorGUILayout.LabelField(NOCOLLIDER_ERR, EditorStyles.helpBox);
                }
                if (!_obj.CanFit)
                {
                    GUI.color = Color.red;
                    EditorGUILayout.LabelField(BOUNDSMISMATCH_ERR, EditorStyles.helpBox);
                }
                if (!_boundedXMin.boolValue && !_boundedXMax.boolValue &&
                    !_boundedYMin.boolValue && !_boundedYMax.boolValue)
                {
                    GUI.color = Color.red;
                    EditorGUILayout.LabelField(UNBOUNDED_ERR, EditorStyles.helpBox);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            _obj = (CameraBounds2D) target;
            _camera = serializedObject.FindProperty("camera");
            _bounds = serializedObject.FindProperty("bounds");
            _boundedXMin = serializedObject.FindProperty("boundedXMin");
            _boundedXMax = serializedObject.FindProperty("boundedXMax");
            _boundedYMin = serializedObject.FindProperty("boundedYMin");
            _boundedYMax = serializedObject.FindProperty("boundedYMax");
        }
    }
}