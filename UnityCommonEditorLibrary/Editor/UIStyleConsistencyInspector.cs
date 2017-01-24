using UnityCommonLibrary.UI;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary
{
	[CustomEditor(typeof(UIStyleConsistency))]
	public class UIStyleConsistencyInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EditorGUILayout.Space();
			if(GUILayout.Button("Full Update"))
			{
				(target as UIStyleConsistency).FullUpdate();
			}
		}
	}
}