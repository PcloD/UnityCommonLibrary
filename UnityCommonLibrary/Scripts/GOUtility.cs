using UnityEngine;

namespace UnityCommonLibrary {
    public static class GOUtility {

        public static void SetLayerRecursive(this GameObject obj, int layer) {
            obj.layer = layer;
            foreach(Transform child in obj.transform) {
                child.gameObject.SetLayerRecursive(layer);
            }
        }

    }
}