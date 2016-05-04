using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary
{
    public static class GameObjectEditorUtilities {
        [MenuItem("GameObject/Create Unrelated Prefab Instance")]
        public static void CreateUnrelatedPrefabInstance() {
            Object.Instantiate(Selection.activeGameObject);
        }

        [MenuItem("GameObject/Create Unrelated Prefab Instance", validate = true)]
        public static bool ValidateCreateUnrelatedPrefabInstance() {
            if(Selection.activeGameObject == null) {
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