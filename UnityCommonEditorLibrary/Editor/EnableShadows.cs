using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityCommonEditorLibrary {
    [InitializeOnLoad]
    public class EnableShadows : Editor {
        static Material mat;

        [MenuItem("Tools/Apply Sprite Material")]
        private static void EditorUpdate() {
            mat = Resources.Load<Material>("Sprite");
            var sprites = FindObjectsOfType<SpriteRenderer>();
            if(mat != null) {
                foreach(var s in sprites) {
                    s.sharedMaterial = mat;
                    s.receiveShadows = true;
                    s.shadowCastingMode = ShadowCastingMode.On;
                }
            }
        }
    }
}