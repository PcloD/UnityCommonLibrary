using UnityCommonLibrary.UI;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary
{
    [CustomEditor(typeof(UiStyleConsistency))]
    public class UiStyleConsistencyInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            if (GUILayout.Button("Full Update"))
            {
                (target as UiStyleConsistency).FullUpdate();
            }
        }
    }
}