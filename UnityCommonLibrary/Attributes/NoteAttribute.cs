using System;
using UnityEngine;

namespace UnityCommonLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class NoteAttribute : PropertyAttribute
    {
        public enum MessageType
        {
            None,
            Info,
            Warning,
            Error
        }

        public readonly string Text;
        public readonly MessageType Type;

        public NoteAttribute(string note) : this(note, MessageType.None) { }

        public NoteAttribute(string text, MessageType type)
        {
            Text = text.Replace("\n", Environment.NewLine);
            Type = type;
        }
    }
}