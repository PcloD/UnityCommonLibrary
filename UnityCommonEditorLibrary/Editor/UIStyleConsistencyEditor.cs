using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary {
    [CustomEditor(typeof(UIStyleConsistency))]
    public class UIStyleConsistencyEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            if(GUILayout.Button("Full Update")) {
                (target as UIStyleConsistency).FullUpdate();
            }
        }

    }
}