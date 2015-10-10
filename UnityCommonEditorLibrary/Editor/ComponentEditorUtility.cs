using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityCommonEditorLibrary {
    public class ComponentEditorUtility {

        [MenuItem("CONTEXT/Component/Move To Top")]
        public static void MoveToTop(MenuCommand cmd) {
            var component = (Component)cmd.context;
            var didMove = ComponentUtility.MoveComponentUp(component);
            while(didMove) {
                didMove = ComponentUtility.MoveComponentUp(component);
            }
        }

        [MenuItem("CONTEXT/Component/Move To Bottom")]
        public static void MoveToBottom(MenuCommand cmd) {
            var component = (Component)cmd.context;
            var didMove = ComponentUtility.MoveComponentDown(component);
            while(didMove) {
                didMove = ComponentUtility.MoveComponentDown(component);
            }
        }

    }
}