using UnityCommonLibrary.Attributes;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary.Inspectors
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            return 0f;
        }

        public override void OnGUI(Rect position)
        {
            GUI.enabled = false;
        }
    }
}