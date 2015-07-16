using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary {
    [CustomEditor(typeof(CameraBounds2D))]
    public class CameraBounds2DEditor : Editor {

        SerializedProperty camera,
                           bounds,
                           targetSprites,
                           gizmosColor,
                           boundedX,
                           boundedY;

        CameraBounds2D cb2d;

        const string CANT_FIT_ERR = "Camera's frustum CANNOT fit into current bounds, camera's position WILL NOT be altered!";
        const string UNBOUNDED_ERR = "Set to unbounded in both axes, camera's position WILL NOT be altered!";

        void OnEnable() {
            cb2d = (CameraBounds2D)target;
            camera = serializedObject.FindProperty("camera");
            bounds = serializedObject.FindProperty("bounds");
            targetSprites = serializedObject.FindProperty("targetSprites");
            gizmosColor = serializedObject.FindProperty("gizmosColor");
            boundedX = serializedObject.FindProperty("boundedX");
            boundedY = serializedObject.FindProperty("boundedY");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Configuration", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(camera);

            EditorGUILayout.Space();

            //Bounds field
            GUI.enabled = targetSprites.arraySize == 0;
            var boundsLabel = targetSprites.arraySize == 0 ? "Bounds" : "Bounds (From Sprites)";
            EditorGUILayout.PropertyField(bounds, new GUIContent(boundsLabel));
            GUI.enabled = true;


            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Misc.", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(boundedX);
            EditorGUILayout.PropertyField(boundedY);
            EditorGUILayout.PropertyField(gizmosColor);

            EditorGUILayout.PropertyField(targetSprites, true);
            EditorGUILayout.Space();
            //Show Errors
            if(!cb2d.canFit) {
                GUI.color = Color.red;
                EditorGUILayout.LabelField(CANT_FIT_ERR, EditorStyles.helpBox);
            }
            if(!boundedX.boolValue && !boundedY.boolValue) {
                GUI.color = Color.red;
                EditorGUILayout.LabelField(UNBOUNDED_ERR, EditorStyles.helpBox);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}