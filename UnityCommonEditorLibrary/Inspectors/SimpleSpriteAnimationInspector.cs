using System;
using UnityCommonLibrary;
using UnityCommonLibrary.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors {
    [CustomEditor(typeof(SimpleSpriteAnimation))]
    public class SimpleSpriteAnimationInspector : Editor {
        private ReorderableList frames;

        private float elementSpacing = 5f;
        private float objFieldHeight;

        private void OnEnable() {
            frames = new ReorderableList(serializedObject, serializedObject.FindProperty("frames"));
            frames.drawElementCallback += DrawFrameElement;
            frames.drawHeaderCallback += (rect) => {
                EditorGUI.LabelField(rect, "Animation Frames");
            };

            objFieldHeight = frames.elementHeight;
            frames.elementHeight = 64f;
        }

        private void DrawFrameElement(Rect rect, int index, bool isActive, bool isFocused) {
            var value = frames.serializedProperty.GetArrayElementAtIndex(index);
            var sprite = value.objectReferenceValue as Sprite;

            var objFieldRect = new Rect(rect);
            objFieldRect.height = objFieldHeight;

            if(sprite != null) {
                var previewRect = new Rect(rect);
                previewRect.width = previewRect.height * sprite.texture.GetAspect();
                EditorGUI.DrawTextureTransparent(previewRect, sprite.texture);

                objFieldRect.x += previewRect.width + elementSpacing;
                objFieldRect.width -= previewRect.width + elementSpacing;
                objFieldRect.y += rect.height - objFieldHeight;
            }
            else {
                objFieldRect.y += rect.height / 2f;
                objFieldRect.y -= objFieldHeight / 2f;
            }
            objFieldRect.height = objFieldHeight;
            EditorGUI.ObjectField(objFieldRect, value, GUIContent.none);
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            serializedObject.Update();
            EditorGUILayout.Separator();
            frames.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

    }
}
