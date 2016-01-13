using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary {
    [CustomPropertyDrawer(typeof(NoteAttribute))]
    public class NoteAttributeDrawer : DecoratorDrawer {
        private NoteAttribute note;
        private MessageType type;
        private float height;

        public override float GetHeight() {
            EnsureNoteData();
            return height;
        }

        public override void OnGUI(Rect position) {
            EnsureNoteData();
            EditorGUI.HelpBox(position, note.text, type);
        }

        private void EnsureNoteData() {
            if(note == null) {
                note = attribute as NoteAttribute;
                type = (MessageType)((int)note.type);
                height = EditorStyles.helpBox.CalcSize(new GUIContent(note.text)).y;
            }
        }
    }
}
