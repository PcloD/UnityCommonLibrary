using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace UnityCommonEditorLibrary
{
    public class UCLEditorUtility
    {
        private static readonly Dictionary<string, bool> foldouts = new Dictionary<string, bool>();
        public static bool Foldout(string key, string display)
        {
            if (!foldouts.ContainsKey(key))
            {
                foldouts.Add(key, true);
            }
            foldouts[key] = EditorGUILayout.Foldout(foldouts[key], display);
            return foldouts[key];
        }
        private static readonly Dictionary<string, AnimBool> fadeGroups = new Dictionary<string, AnimBool>();
        public static bool BeginFadeGroup(string key)
        {
            if (!fadeGroups.ContainsKey(key))
            {
                fadeGroups.Add(key, new AnimBool(true));
            }
            fadeGroups[key].target = EditorGUILayout.BeginFadeGroup(fadeGroups[key].faded);
            return fadeGroups[key].value;
        }
        public static void EndFadeGroup()
        {
            EditorGUILayout.EndFadeGroup();
        }
    }
}