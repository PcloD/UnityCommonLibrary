using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary {
    [CustomEditor(typeof(SpriteMeasurer))]
    public class SpriteMeasurerEditor : Editor {

        public override void OnInspectorGUI() {
            var obj = target as SpriteMeasurer;
            var p2d = obj.p2d;
            var renderer = obj.renderer;
            obj.hideFlags = HideFlags.DontSaveInBuild;

            EditorGUILayout.LabelField("Collider Size: " + p2d.bounds.size);
            EditorGUILayout.LabelField(string.Format("Size in Pixels: {0}x{1}", renderer.sprite.texture.width, renderer.sprite.texture.height));
            EditorGUILayout.LabelField("Pixels per Unit: " + renderer.sprite.pixelsPerUnit);
        }

    }
}