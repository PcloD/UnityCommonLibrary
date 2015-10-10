using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary {

    [CustomEditor(typeof(Ruler))]
    public class RulerUtility : Editor {
        [MenuItem("GameObject/Create Other/Ruler")]
        public static void Create() {
            var obj = new GameObject("RULER", typeof(Ruler));
            obj.hideFlags = HideFlags.DontSaveInBuild;
            Selection.activeGameObject = obj;
        }

        void OnSceneGUI() {
            var obj = target as Ruler;
            if(obj.endTransform == null) {
                obj.end = Handles.DoPositionHandle(obj.end, Quaternion.identity);
            }
            Handles.Label((obj.transform.position + obj.selectedEnd) / 2f, obj.distance.ToString() + " units", EditorStyles.helpBox);
            if(GUI.changed) {
                EditorUtility.SetDirty(target);
            }
        }

    }

}