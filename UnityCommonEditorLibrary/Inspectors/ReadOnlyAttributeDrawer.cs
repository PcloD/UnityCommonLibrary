using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary {
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : DecoratorDrawer {

        public override float GetHeight() {
            return 0f;
        }

        public override void OnGUI(Rect position) {
            GUI.enabled = false;
        }

    }
}