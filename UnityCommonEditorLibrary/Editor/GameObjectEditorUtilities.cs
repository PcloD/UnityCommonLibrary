using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary
{
    public static class GameObjectEditorUtilities
    {
        [MenuItem("GameObject/Create Unlinked Instance")]
        public static void CreateUnlinkedInstance()
        {
            Object.Instantiate(Selection.activeGameObject);
        }

        [MenuItem("GameObject/Create Unlinked Instance", validate = true)]
        public static bool ValidateCreateUnlinkedInstance()
        {
            if (Selection.activeGameObject == null)
            {
                return false;
            }
            var type = PrefabUtility.GetPrefabType(Selection.activeGameObject);
            return type == PrefabType.PrefabInstance ||
                   type == PrefabType.Prefab ||
                   type == PrefabType.MissingPrefabInstance ||
                   type == PrefabType.DisconnectedPrefabInstance;
        }
    }
}