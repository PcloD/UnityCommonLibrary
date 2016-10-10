using System.Text;
using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary
{
	[CustomEditor(typeof(TriggerContents))]
	public class TriggerContentsInspector : Editor
	{
		private TriggerContents contents;
		private Vector2 scrollValue;
		private StringBuilder sb = new StringBuilder();

		private void OnEnable()
		{
			contents = target as TriggerContents;
		}
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if(!Application.isPlaying)
			{
				return;
			}
			EditorGUILayout.LabelField("Contents:");
			//scrollValue = EditorGUILayout.BeginScrollView(scrollValue, GUILayout.Height(300f));
			sb.Length = 0;
			foreach(var c in contents.contents)
			{
				EditorGUILayout.LabelField(c ? c.name : "NULL");
			}
			//EditorGUILayout.EndScrollView();
			Repaint();
		}
	}
}
