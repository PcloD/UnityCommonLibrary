using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityCommonEditorLibrary
{
    public class ComponentEditorUtility
    {
        [MenuItem("CONTEXT/Component/Move to Top")]
        private static void MoveToTop(MenuCommand c)
        {
            var component = c.context as Component;
            var didMove = ComponentUtility.MoveComponentUp(component);
            while (didMove)
            {
                didMove = ComponentUtility.MoveComponentUp(component);
            }
        }

        [MenuItem("CONTEXT/Component/Move to Bottom")]
        private static void MoveToBottom(MenuCommand c)
        {
            var component = c.context as Component;
            var didMove = ComponentUtility.MoveComponentDown(component);
            while (didMove)
            {
                didMove = ComponentUtility.MoveComponentDown(component);
            }
        }

        [MenuItem("CONTEXT/Component/Collapse All")]
        private static void CollapseAll(MenuCommand cmd)
        {
            var component = cmd.context as Component;
            if (component)
            {
                FoldAllOnGameObject(component.gameObject, false);
            }
        }

        [MenuItem("CONTEXT/Component/Expand All")]
        private static void ExpandAll(MenuCommand cmd)
        {
            var component = cmd.context as Component;
            if (component)
            {
                FoldAllOnGameObject(component.gameObject, true);
            }
        }

        private static void FoldAllOnGameObject(GameObject obj, bool enabled)
        {
            var inspectorWindow = EditorWindow.focusedWindow;
            var tracker = (ActiveEditorTracker) inspectorWindow
                .GetType().GetMethod("GetTracker").Invoke(inspectorWindow, null);
            for (var i = 0; i < tracker.activeEditors.Length; i++)
            {
                var cmp = tracker.activeEditors[i].target as Component;
                if (cmp && cmp.gameObject == obj)
                {
                    tracker.SetVisible(i, enabled ? 1 : 0);
                    InternalEditorUtility.SetIsInspectorExpanded(cmp, enabled);
                }
            }
        }
    }
}