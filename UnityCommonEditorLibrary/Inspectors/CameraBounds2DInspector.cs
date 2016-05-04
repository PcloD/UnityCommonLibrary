using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors
{
    [CustomEditor(typeof(CameraBounds2D))]
    public class CameraBounds2DInspector : Editor {

        SerializedProperty camera,
                           bounds,
                           boundedXMin,
                           boundedXMax,
                           boundedYMin,
                           boundedYMax;

        CameraBounds2D obj;

        const string NOCAM_ERR = "There is no camera assigned!";
        const string BOUNDSMISMATCH_ERR = "Camera's frustum CANNOT fit into current bounds, camera's position WILL NOT be altered!";
        const string UNBOUNDED_ERR = "Set to unbounded in all directions, camera's position WILL NOT be altered!";
        const string NOCOLLIDER_ERR = "No collider provided, camera's position WILL NOT be altered!";

        bool isBoundsNull;

        void OnEnable() {
            obj = (CameraBounds2D)target;
            camera = serializedObject.FindProperty("camera");
            bounds = serializedObject.FindProperty("bounds");
            boundedXMin = serializedObject.FindProperty("boundedXMin");
            boundedXMax = serializedObject.FindProperty("boundedXMax");
            boundedYMin = serializedObject.FindProperty("boundedYMin");
            boundedYMax = serializedObject.FindProperty("boundedYMax");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Configuration", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(camera);
            EditorGUILayout.PropertyField(bounds);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Misc.", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(boundedXMin);
            EditorGUILayout.PropertyField(boundedXMax);
            EditorGUILayout.PropertyField(boundedYMin);
            EditorGUILayout.PropertyField(boundedYMax);
            EditorGUILayout.Space();

            isBoundsNull = Equals(camera.objectReferenceValue, null);
            //Show Errors
            if(isBoundsNull) {
                GUI.color = Color.red;
                EditorGUILayout.LabelField(NOCAM_ERR, EditorStyles.helpBox);
            }
            else {
                if(Equals(bounds.objectReferenceValue, null)) {
                    GUI.color = Color.red;
                    EditorGUILayout.LabelField(NOCOLLIDER_ERR, EditorStyles.helpBox);
                }
                if(!obj.canFit) {
                    GUI.color = Color.red;
                    EditorGUILayout.LabelField(BOUNDSMISMATCH_ERR, EditorStyles.helpBox);
                }
                if(!boundedXMin.boolValue && !boundedXMax.boolValue && !boundedYMin.boolValue && !boundedYMax.boolValue) {
                    GUI.color = Color.red;
                    EditorGUILayout.LabelField(UNBOUNDED_ERR, EditorStyles.helpBox);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

    }
}