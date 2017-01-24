using System.Collections.Generic;
using UnityEditor;

namespace UnityCommonEditorLibrary
{
	public class UCLEditorUtility
	{
		private static readonly Dictionary<string, bool> foldouts = new Dictionary<string, bool>();
		public static bool Foldout(string key, string display)
		{
			if(!foldouts.ContainsKey(key))
			{
				foldouts.Add(key, true);
			}
			foldouts[key] = EditorGUILayout.Foldout(foldouts[key], display);
			return foldouts[key];
		}
	}
}