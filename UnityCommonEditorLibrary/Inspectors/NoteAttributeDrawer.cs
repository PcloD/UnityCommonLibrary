using UnityCommonLibrary.Attributes;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors
{
    [CustomPropertyDrawer(typeof(NoteAttribute))]
    public class NoteAttributeDrawer : DecoratorDrawer
    {
        private float _height;
        private NoteAttribute _note;
        private MessageType _type;

        public override float GetHeight()
        {
            EnsureNoteData();
            return _height;
        }

        public override void OnGUI(Rect position)
        {
            EnsureNoteData();
            EditorGUI.HelpBox(position, _note.Text, _type);
        }

        private void EnsureNoteData()
        {
            if (_note == null)
            {
                _note = attribute as NoteAttribute;
                _type = (MessageType) (int) _note.Type;
                _height = EditorStyles.helpBox.CalcSize(new GUIContent(_note.Text)).y;
            }
        }
    }
}