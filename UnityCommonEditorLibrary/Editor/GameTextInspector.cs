using System;
using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary {
    [CustomEditor(typeof(GameText))]
    public class GameTextInspector : Editor {

        Vector2 scroll;

        public override void OnInspectorGUI() {
            var obj = target as GameText;
            obj.category = EditorGUILayout.TextField("Category:", obj.category);

            if(GUILayout.Button("+")) {
                obj.lines.Add(new GameTextLine());
                return;
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);

            //Show all lines
            int index = 0;
            foreach(var l in obj.lines) {
                EditorGUILayout.LabelField("ID:", index.ToString());
                index++;
                l.uniqueName = EditorGUILayout.TextField("Unique Name:", l.uniqueName);
                var dict = l.dict;

                //Ensure line has all languages
                foreach(Language lang in Enum.GetValues(typeof(Language))) {
                    if(!l.dict.ContainsKey(lang)) {
                        l.dict.Add(lang, string.Empty);
                    }
                }

                //Show text fields for each language
                EditorGUI.indentLevel = 1;
                for(int i = 0; i < l.dict.length; i++) {
                    var pair = l.dict[i];
                    l.dict.Set(pair.Key, EditorGUILayout.TextField(pair.Key.ToString() + ": ", pair.Value));
                }
                EditorGUI.indentLevel = 0;

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndScrollView();
        }

    }
}