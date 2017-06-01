using System.Linq;
using UnityCommonLibrary;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityCommonEditorLibrary
{
    public class ComponentEditorUtility
    {
        [MenuItem("CONTEXT/Component/Alphabetize Components")]
        private static void AlphabetizeComponents(MenuCommand cmd)
        {
            var component = cmd.context as Component;
            var obj = component.gameObject;
            var components = obj.GetComponents<Component>().ToList();
            components.RemoveAll(c => c is Transform);
            components.RemoveAll(c => c is RectTransform);
            components.Sort((c1, c2) => c1.GetType().Name.CompareTo(c2.GetType().Name));
            for (int i = 0; i < components.Count; i++)
            {
                var target = components[i];
                var targetIndex = i + 1;
                while (true)
                {
                    var currentComponents = obj.GetComponents<Component>().ToList();
                    currentComponents.RemoveAll(c => c is Transform);
                    currentComponents.RemoveAll(c => c is RectTransform);
                    var currentIndex = currentComponents.IndexOf(target);
                    if (currentIndex == -1)
                    {
                        UCLCore.Logger.LogError("", "COMPONENT NOT FOUND: " + target);
                        break;
                    }
                    if (targetIndex > currentIndex)
                    {
                        if (!ComponentUtility.MoveComponentDown(target))
                        {
                            break;
                        }
                    }
                    else if (targetIndex < currentIndex)
                    {
                        if (!ComponentUtility.MoveComponentUp(target))
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

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