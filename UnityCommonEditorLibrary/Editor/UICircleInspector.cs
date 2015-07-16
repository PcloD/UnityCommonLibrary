using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary {
    /// <summary>
    /// User interface circle inspector.
    /// https://bitbucket.org/Elforama/uicircle
    /// </summary>
    [CustomEditor(typeof(UICircle))]
    public class UICircleInspector : Editor {

        public override void OnInspectorGUI() {
            serializedObject.Update();

            var gradient = serializedObject.FindProperty("gradient");

            var radius = serializedObject.FindProperty("radius");
            var angle = serializedObject.FindProperty("angle");
            var angleOffset = serializedObject.FindProperty("angleOffset");
            var flip = serializedObject.FindProperty("flip");
            var thickness = serializedObject.FindProperty("thickness");
            var subdivisions = serializedObject.FindProperty("subdivisions");

            var glow = serializedObject.FindProperty("glow");
            var glowThickness = serializedObject.FindProperty("glowThickness");

            var uiCircle = (UICircle)target;

            EditorGUILayout.LabelField("Appearence", EditorStyles.boldLabel);

            //Set up the appearance section
            uiCircle.material = (Material)EditorGUILayout.ObjectField(new GUIContent("Material", "Sets the material"), uiCircle.material, typeof(Material), true);
            EditorGUILayout.PropertyField(gradient, new GUIContent("Use Gradient", "Turns gradient on/off"));
            if(gradient.boolValue) {//if using gradient, show secondary color
                uiCircle.color = EditorGUILayout.ColorField(new GUIContent("Outer Color", "Sets the outer perimeter color"), uiCircle.color);
                uiCircle.gradientColor = EditorGUILayout.ColorField(new GUIContent("Inner Color", "Sets the inner/center color"), uiCircle.gradientColor);
            }
            else //if not using gradient, just so single color
                uiCircle.color = EditorGUILayout.ColorField(new GUIContent("Color", "Sets the circles color"), uiCircle.color);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Dimensions", EditorStyles.boldLabel);

            //Set up the dimensions section
            EditorGUILayout.PropertyField(radius, new GUIContent("Radius", "The radius of the circle"));
            EditorGUILayout.Slider(angle, 0, 360, new GUIContent("Angle", "How many degrees of the circle to show"));
            EditorGUILayout.Slider(angleOffset, 0, 359, new GUIContent("Angle Offset", "What angle the circle starts at"));
            EditorGUILayout.PropertyField(flip, new GUIContent("Flip", "Draw circle in opposite direction"));
            EditorGUILayout.Slider(thickness, 0, 1, new GUIContent("Thickness", "Determines the size of the center hole."));
            EditorGUILayout.IntSlider(subdivisions, 1, 6, new GUIContent("Subdivisions", "Sets the level of detail/smoothness of the circle"));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Glow", EditorStyles.boldLabel);

            //Set up the glow section
            EditorGUILayout.PropertyField(glow, new GUIContent("Glow", "Set glow on or off"));
            uiCircle.glowColor = EditorGUILayout.ColorField(new GUIContent("Glow", "Sets glow color"), uiCircle.glowColor);
            EditorGUILayout.Slider(glowThickness, .01f, .25f, new GUIContent("Glow Thickness", "Sets size of glow"));

            serializedObject.ApplyModifiedProperties();

            if(GUI.changed) {
                EditorUtility.SetDirty(target);
                //request rebuild so that gradient color is properly updated
                uiCircle.OnRebuildRequested();
            }
        }
    }
}