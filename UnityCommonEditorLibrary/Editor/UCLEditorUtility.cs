using System.Collections.Generic;
using UnityEditor;

namespace UnityCommonEditorLibrary
{
    public class UclEditorUtility
    {
        private static readonly Dictionary<string, bool> Foldouts =
            new Dictionary<string, bool>();

        public static bool Foldout(string key, string display)
        {
            if (!Foldouts.ContainsKey(key))
            {
                Foldouts.Add(key, true);
            }
            Foldouts[key] = EditorGUILayout.Foldout(Foldouts[key], display);
            return Foldouts[key];
        }
    }
}