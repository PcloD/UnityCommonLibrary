using UnityEngine;

namespace UnityCommonLibrary {
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class NoteAttribute : PropertyAttribute {
        public readonly string text;
        public readonly MessageType type;

        public NoteAttribute(string note) : this(note, MessageType.None) { }

        public NoteAttribute(string text, MessageType type) {
            this.text = text.Replace("\n", System.Environment.NewLine);
            this.type = type;
        }

        public enum MessageType {
            None,
            Info,
            Warning,
            Error
        }
    }
}
