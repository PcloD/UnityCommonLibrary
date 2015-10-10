using UnityEngine;

namespace UnityCommonLibrary {
    public static class GOUtility {

        public static void SetLayerRecursive(this GameObject obj, int layer) {
            obj.layer = layer;
            foreach(Transform child in obj.transform) {
                child.gameObject.SetLayerRecursive(layer);
            }
        }

        public static string GetPath(GameObject obj) {
            var path = "/" + obj.name;
            while(obj.transform.parent != null) {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }

        public static void Toggle(bool enabled, params GameObject[] gameObjects) {
            foreach(var go in gameObjects) {
                if(go != null) {
                    go.SetActive(enabled);
                }
            }
        }

    }
}