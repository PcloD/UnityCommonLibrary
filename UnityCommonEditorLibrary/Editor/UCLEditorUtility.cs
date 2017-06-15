using System.Collections.Generic;
using UnityEditor;

namespace UnityCommonEditorLibrary
{
    public class UclEditorUtility
    {
        private static readonly Dictionary<string, bool> _foldouts =
            new Dictionary<string, bool>();

        public static bool Foldout(string key, string display)
        {
            if (!_foldouts.ContainsKey(key))
            {
                _foldouts.Add(key, true);
            }
            _foldouts[key] = EditorGUILayout.Foldout(_foldouts[key], display);
            return _foldouts[key];
        }
    }
}