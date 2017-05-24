using System.Text;
using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary
{
    [CustomEditor(typeof(TriggerContents))]
    public class TriggerContentsInspector : Editor
    {
        private readonly StringBuilder _sb = new StringBuilder();
        private TriggerContents _contents;
        private Vector2 _scrollValue;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!Application.isPlaying)
            {
                return;
            }
            EditorGUILayout.LabelField("Contents:");
            //scrollValue = EditorGUILayout.BeginScrollView(scrollValue, GUILayout.Height(300f));
            _sb.Length = 0;
            foreach (var c in _contents.Contents)
            {
                EditorGUILayout.LabelField(c ? c.name : "NULL");
            }
            //EditorGUILayout.EndScrollView();
            Repaint();
        }

        private void OnEnable()
        {
            _contents = target as TriggerContents;
        }
    }
}