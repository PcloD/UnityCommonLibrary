using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors
{
    [CustomEditor(typeof(ParallaxCamera))]
    public class ParallaxCameraInspector : Editor
    {
        private ParallaxCamera _options;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Save Position"))
            {
                _options.SavePosition();
                EditorUtility.SetDirty(_options);
            }

            if (GUILayout.Button("Restore Position"))
            {
                _options.RestorePosition();
            }
        }

        private void Awake()
        {
            _options = (ParallaxCamera) target;
        }
    }
}