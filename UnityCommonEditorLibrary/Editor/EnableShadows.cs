using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[InitializeOnLoad]
public class EnableShadows : UnityEditor.Editor {
    static Material mat;

    static EnableShadows() {
        EditorApplication.update += EditorUpdate;
    }

    private static void EditorUpdate() {
        mat = Resources.Load<Material>("Sprite");
        var sprites = FindObjectsOfType<SpriteRenderer>();
        foreach(var s in sprites) {
            s.sharedMaterial = mat;
            s.receiveShadows = true;
            s.shadowCastingMode = ShadowCastingMode.On;
        }
    }
}